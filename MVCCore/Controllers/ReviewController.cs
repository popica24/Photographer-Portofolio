using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models;
using MVCCore.Services.Abstract;
using MVCCore.Services.Concrete;

namespace MVCCore.Controllers
{
    [Authorize]
    [Route("reviewmanager")]
    public class ReviewController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo<ReviewModel> _reviewRepo;

        public ReviewController(IRepo<ReviewModel> reviewRepo, ILogger<HomeController> logger)
        {
            _reviewRepo = reviewRepo;
            _logger = logger;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var list = await _reviewRepo.GetAll(); 
            return View(list);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ReviewModel model)
        {
            try
            {
                await _reviewRepo.CreateAsync(model);
                _logger.LogTrace("Review created");
                TempData["Success"] = "Review created successfully !";
            }catch (Exception ex)
            {
                TempData["Error"] = "Revoiew creation failed with the exception " + ex.Message + "\nContact developer";
                _logger.LogError("Revoiew creation failed with the exception " + ex.Message);
            }
            return RedirectToAction("Index");
        }

        [Route("delete")]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return RedirectToAction("Index");
            var album = await _reviewRepo.GetAsync(id);
            return View(album);
        }


        [Route("delete/{Id}/confirmed")]
        public async Task<IActionResult> DeleteConfirmed(ReviewModel request)
        {
            if (request.Id == null) return RedirectToAction("Index");
            try
            {
                await _reviewRepo.DeleteAsync(request);
                TempData["Success"] = "Review deleted successfully !";
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Review deletion failed with the exception " + ex.Message + "\nContact developer";
                _logger.LogError("Review deletion failed with the exception " + ex.Message);
            }
            
            return RedirectToAction("Index");
        }
    }
}
