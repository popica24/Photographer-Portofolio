using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using MVCCore.Models;
using MVCCore.Models.Enumerations;
using MVCCore.Services.Abstract;
using MVCCore.Services.Concrete;
using System.Diagnostics;

namespace MVCCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepo<StatsModel> _statsRepo;
        private readonly IRepo<ReviewModel> _reviewRepo;
        private readonly IRepo<CoverModel> _coverRepo;
        private readonly IExtendedAlbumOptions _albumOptions;
        private readonly IEmailHelper _emailHelper;

        public HomeController(ILogger<HomeController> logger, IRepo<AlbumModel> albumRepo, IExtendedAlbumOptions albumOptions, IRepo<StatsModel> statsRepo, IRepo<ReviewModel> reviewRepo, IEmailHelper emailHelper, IRepo<CoverModel> coverRepo)
        {
            _albumOptions = albumOptions;
            _statsRepo = statsRepo;
            _reviewRepo = reviewRepo;
            _emailHelper = emailHelper;
            _coverRepo = coverRepo;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var album = await _albumOptions.CategorySearchAsync(Category.TopFive);
            var stats = await _statsRepo.GetAll();
            var reviews  = await _reviewRepo.GetAll();
            var covers = await _coverRepo.GetAll();
            var data = new Tuple<AlbumModel, StatsModel, List<ReviewModel>,List<CoverModel>>(album.First(), stats.First(), reviews,covers);
            return View(data);
        }


        [Route("{categoryId}")]
        public async Task<IActionResult> DisplayAlbums(Category categoryId)
        {
            switch (categoryId)
            {
                case Category.Locations:
                    var locations = await _albumOptions.LoadCategory(Category.Locations);
                    return RedirectToAction("LoadAlbum", new { categoryId = categoryId,albumId = locations.Id });
                case Category.Portraits:
                    var portraits = await _albumOptions.LoadCategory(Category.Portraits);
                    return RedirectToAction("LoadAlbum", new { categoryId = categoryId, albumId = portraits.Id });
                case Category.Studio:
                    var studio = await _albumOptions.LoadCategory(Category.Studio);
                    return RedirectToAction("LoadAlbum", new { categoryId = categoryId, albumId = studio.Id });
                default:
                    var albums = await _albumOptions.CategorySearchAsync(categoryId);
                    return View(albums);
            }
        }

        [Route("{categoryId}/{albumId}")]
        public async Task<IActionResult> LoadAlbum(string albumId,int pageIndex = 1, int pageSize = 8)
        {

            var albumPage = await _albumOptions.LoadPaginated(albumId, pageIndex, pageSize);
            return View(albumPage);
        }

        [HttpPost("")]
        public async Task SendMail(string name, string email, DateTime eventDate, string location, string exactLocation, string aboutYou, string howFound, string socialMedia, string message)
        {
            var messageBody = $"Nume client : {name}\nEmail client : {email}\nData Eveniment : {eventDate.ToShortDateString()}\nLocatie : {location}\nLocatie Exacta : {exactLocation}\nDespre {name} : {aboutYou}\n{name} te-a gasit prin intermediul {howFound}\nSocial Media : {socialMedia}\nMesaj pentru tine : {message}";
            ContactModel contactModel = new ContactModel() { Name = name, Email = email,Subject = aboutYou, Message = messageBody };
           await _emailHelper.SendAsync(contactModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}