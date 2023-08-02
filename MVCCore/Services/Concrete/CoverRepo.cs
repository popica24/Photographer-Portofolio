using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Options;
using MVCCore.Services.Abstract;
using Microsoft.Extensions.Options;

namespace MVCCore.Services.Concrete
{
    public class CoverRepo : IRepo<CoverModel>
    {

        private readonly ApplicationDbContext _context;
        private readonly AzureOptions _azureOptions;
        private readonly AzureContainers _azureContainers;

        public CoverRepo(ApplicationDbContext context, IOptions<AzureOptions> azureOptions, IOptions<AzureContainers> azureContainers)
        {
            _context = context;
            _azureOptions = azureOptions.Value;
            _azureContainers = azureContainers.Value;
        }

        public Task CreateAsync(CoverModel entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CoverModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CoverModel>> GetAll()
        {
           return await _context.Covers.ToListAsync();
        }

        public async Task<CoverModel> GetAsync(string id)
        {
            return await _context.Covers.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task UpdateAsync(CoverModel entity)
        {
            var oldEntity = await GetAsync(entity.ID);
            var newDbPath = Guid.NewGuid().ToString();
            oldEntity.DbPath = _azureContainers.Albums + newDbPath;
            await _context.SaveChangesAsync();
              await UploadToAzure(entity.Thumbnail, newDbPath);

        }

        private async Task UploadToAzure(IFormFile photo, string name)
        {
            using MemoryStream fileUploadStream = new MemoryStream();
            photo.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;
            BlobContainerClient blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString, _azureOptions.AlbumContainer);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/bitmap"
                }
            }, cancellationToken: default);
        }
    }
}
