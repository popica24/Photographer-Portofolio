using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models;
using MVCCore.Services.Abstract;

namespace MVCCore.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo<AlbumModel> _albumRepo;
        private readonly IExtendedAlbumOptions _albumOptions;
        public AdminController(ILogger<HomeController> logger, IRepo<AlbumModel> albumRepo, IExtendedAlbumOptions albumOptions)
        {
            _logger = logger;
            _albumRepo = albumRepo;
            _albumOptions = albumOptions;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }


        

        


       


        
    }
}
