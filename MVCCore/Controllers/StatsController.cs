using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Services.Abstract;
using MVCCore.Services.Concrete;

namespace MVCCore.Controllers
{
    [Authorize]
    [Route("statsmanager")]
    public class StatsController : Controller
    {
        private IRepo<StatsModel> _statsRepo;

        public StatsController(IRepo<StatsModel> statsRepo)
        {
            _statsRepo = statsRepo;  
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("")]
        public async Task<IActionResult> Index(StatsModel model)
        {
            await _statsRepo.UpdateAsync(model);
            return RedirectToAction("Index");
        }
    }
}
