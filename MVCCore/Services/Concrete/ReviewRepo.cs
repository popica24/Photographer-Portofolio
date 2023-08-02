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
    public class ReviewRepo : IRepo<ReviewModel>
    {
        private readonly AzureOptions _azureOptions;
        private readonly AzureContainers _azureContainers;
        private ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context,IOptions<AzureOptions> azureOptions, IOptions<AzureContainers> azureContainers)
        {
            _context = context;
            _azureOptions = azureOptions.Value;
            _azureContainers = azureContainers.Value;

        }

        public async Task CreateAsync(ReviewModel entity)
        {
           if(entity.Image != null)
            {
               var name = await UploadToDatabaseAsync();
                await UploadToAzure(entity.Image,name);
            }
        }

        public async Task DeleteAsync(ReviewModel model)
        {
            _context.Reviews.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReviewModel>> GetAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<ReviewModel> GetAsync(string id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateAsync(ReviewModel entity)
        {
            throw new NotImplementedException();
        }
        private async Task UploadToAzure(IFormFile photo, string name)
        {
            using MemoryStream fileUploadStream = new MemoryStream();
            photo.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;
            BlobContainerClient blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString, _azureOptions.ReviewContainer);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/bitmap"
                }
            }, cancellationToken: default);
        }

        private async Task<string> UploadToDatabaseAsync()
        {
            var id = Guid.NewGuid().ToString();
            await _context.Reviews.AddAsync(
                new ReviewModel()
                {
                    Id = id,
                    DbPhoto = _azureContainers.Reviews + id
                });
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
