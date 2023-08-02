using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Models.Enumerations;

namespace MVCCore.Services.Abstract
{
    public interface IExtendedAlbumOptions
    {
        Task<List<AlbumModel>> CategorySearchAsync(Category parameter);
        Task<AlbumModel> LoadCategory(Category parameter);
        Task DeletePhoto (string albumId,string  photoId);
        Task<PaginatedList<PhotoModel>> LoadPaginated(string albumId, int pageIndex, int pageSize);
    }
}
