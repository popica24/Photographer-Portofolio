using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models;
using MVCCore.Services.Abstract;

namespace MVCCore.Controllers
{
    [Authorize]
    [Route("manager")]
    public class AlbumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo<AlbumModel> _albumRepo;
        private readonly IExtendedAlbumOptions _albumOptions;
        public AlbumController(ILogger<HomeController> logger, IRepo<AlbumModel> albumRepo, IExtendedAlbumOptions albumOptions)
        {
            _logger = logger;
            _albumRepo = albumRepo;
            _albumOptions = albumOptions;
        }

        [Route("managerportal")]
        public async Task<IActionResult> Index()
        {
            var list = await _albumRepo.GetAll();
            return View(list);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("contentmanager")]
        [Route("contentmanager/{id?}")]
        public async Task<IActionResult> ContentManager(string id)
        {
            if (id == null) return RedirectToAction("Index");
            var album = await _albumRepo.GetAsync(id);
            return View(album);
        }


        [Route("edit")]
        [Route("edit/{id?}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var album = await _albumRepo.GetAsync(id);
            return View(album);
        }
        [Route("delete")]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return RedirectToAction("Index");
            var album = await _albumRepo.GetAsync(id);
            return View(album);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(AlbumModel model)
        {
            try
            {
                await _albumRepo.UpdateAsync(model);
                _logger.LogTrace("Album with the id " + model.Id + " edited");
                TempData["Success"] = "Album with the id " + model.Id + "edited successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Album editing failed with the exception " + ex.Message + "\nContact developer";
                _logger.LogError("Album editing failed with the exception " + ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(AlbumModel model)
        {

            try
            {
                await _albumRepo.CreateAsync(model);
                _logger.LogTrace("Album with the name " + model.Name + " created");
                TempData["Success"] = "Album created successfully !";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Album creation failed with the exception " + ex.Message + "\nContact developer";
                _logger.LogError("Album creation failed with the exception " + ex.Message);
            }

            return RedirectToAction("Index");


        }

        [HttpPost("delete/{albumId}/{photoId}/confirmed")]
        public async Task DeletePhotoConfirmed(string albumId, string photoId)
        {
            try
            {
                await _albumOptions.DeletePhoto(albumId, photoId);
                _logger.LogTrace("Album with the id " + albumId + " removed the photo with the id " + photoId);
                TempData["Success"] = "Album with the id " + albumId + " removed the photo with the id " + photoId;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Album with the id " + albumId + " couldn't delete the photo with the id " + photoId;
                _logger.LogError("Album with the id " + albumId + " couldn't delete the photo with the id " + photoId + "with exception : " + ex.Message);
            }
        }

        [Route("delete/{id}/confirmed")]
        public async Task<IActionResult> DeleteConfirmed(AlbumModel model)
        {
            if(model.Category == Models.Enumerations.Category.Studio || model.Category == Models.Enumerations.Category.Locations || model.Category == Models.Enumerations.Category.TopFive || model.Category == Models.Enumerations.Category.Portraits)
            {
                TempData["Error"] = "Can't delete album of category " + model.Category.ToString();
                return RedirectToAction("Index");
            }
            try
            {
                await _albumRepo.DeleteAsync(model);
                TempData["Success"] = "Album deleted successfully !";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Album deletion failed with the exception " + ex.Message + "\nContact developer";
                _logger.LogError("Album deletion failed with the exception " + ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
