using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Models.Enumerations;
using MVCCore.Options;
using MVCCore.Services.Abstract;
using NuGet.Packaging;

namespace MVCCore.Services.Concrete
{
    public class AlbumRepo : IRepo<AlbumModel> , IExtendedAlbumOptions
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureOptions _azureOptions;
        private readonly AzureContainers _azureContainers;
        public AlbumRepo(ApplicationDbContext context, IOptions<AzureOptions> azureOptions,IOptions<AzureContainers> azureContainers)
        {
            _context = context;
            _azureOptions = azureOptions.Value;
            _azureContainers = azureContainers.Value;
        }

        public async Task<List<AlbumModel>> CategorySearchAsync(Category parameter)
        {
            if(parameter == Category.Portraits || parameter == Category.TopFive || parameter == Category.Studio || parameter == Category.Locations)
                return await _context.Albums
       .Include(a => a.DbPhotos.Where(a=>!a.IsThumbnail))
       .Where(x => x.Category == parameter)
       .ToListAsync();
            return await _context.Albums
        .Include(a => a.DbPhotos.Where(x=>x.IsThumbnail))
        .Where(x => x.Category == parameter)
        .ToListAsync();
        }

        public async Task CreateAsync(AlbumModel entity)
        {
            string albumId = Guid.NewGuid().ToString();
            
                entity.Id = albumId;
                await _context.AddAsync(entity);

                await _context.SaveChangesAsync();

                if (entity.Images != null)
                {
                    await SetThumbnail(entity.Images.First(), albumId);
                }
               
                await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AlbumModel model)
        {
            _context.Albums.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhoto(string albumId, string photoId)
        {
            var photo = _context.Photos.FirstOrDefault(x=>x.AlbumId == albumId && x.Id == photoId);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();  
        }

        public async Task<List<AlbumModel>> GetAll()
        {
            return await _context.Albums.Include(x => x.DbPhotos).ToListAsync();
        }

        public async Task<AlbumModel> GetAsync(string id)
        {
            return await _context.Albums.Include(x => x.DbPhotos).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AlbumModel> LoadCategory(Category parameter)
        {
            return await _context.Albums.Include(x => x.DbPhotos).Where(x => x.Category == parameter).FirstAsync();
        }

        public async Task<PaginatedList<PhotoModel>> LoadPaginated(string albumId, int pageIndex, int pageSize = 6)
        {
            var query = _context.Photos.Where(x=> x.AlbumId == albumId).OrderBy(a=>a.Position);
            var gamePage = await PaginatedList<PhotoModel>.CreateAsync(query, pageIndex, pageSize);
            return gamePage;
        }

        public async Task UpdateAsync(AlbumModel entity)
        {

            var oldEntity = await _context.Albums.Include(x => x.DbPhotos).FirstOrDefaultAsync(x => x.Id == entity.Id);
            int order = 0;
            if (oldEntity != null)
            {
                if (oldEntity.DbPhotos.Any())
                {
                    order = oldEntity.DbPhotos.OrderBy(x => x.Position).Last().Position;
                }
                if (entity.Name != null)
                {
                    oldEntity.Name = entity.Name;
                }
                if (entity.Category != 0)
                {
                    oldEntity.Category = entity.Category;
                }
                if (entity.Images != null && entity.Images.Count > 0)
                {
                    try
                    {
                        var result = await UploadToDatabaseAsync(oldEntity.Id, order + 1);

                        await UploadToAzure(entity.Images.First(), result);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task SetThumbnail(IFormFile photo, string albumId)
        {
            if(photo != null)
            {
                var thumb = await _context.Photos.FirstOrDefaultAsync(x => x.AlbumId == albumId && x.IsThumbnail);

                var thumbname = Guid.NewGuid().ToString();

                if (thumb != null)
                {
                    thumb.IsThumbnail = false;
                }

                thumb = new PhotoModel()
                {
                        Id = thumbname,
                        IsThumbnail = true,
                        AlbumId = albumId,
                        DbPath = _azureContainers.Albums + thumbname
                };
              
                await UploadToAzure(photo, thumbname);

                await _context.Photos.AddAsync(thumb);

                await _context.SaveChangesAsync();
            }
        }

        private async Task UploadToAzure(IFormFile photo,string name)
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
        
        private async Task<string> UploadToDatabaseAsync(string albumId,int order)
        {
            string uniquePhotoName = Guid.NewGuid().ToString();
            var a = await _context.Photos.AddAsync(
                new PhotoModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    DbPath = _azureContainers.Albums + uniquePhotoName,
                    AlbumId = albumId,
                    Position = order
                });
            return uniquePhotoName;
        }
    }
}
