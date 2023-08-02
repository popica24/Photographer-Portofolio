using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models;
using MVCCore.Models.Enumerations;
using MVCCore.Services.Abstract;

namespace MVCCore.Controllers
{
    [Authorize]
    [Route("cover-portal")]
    public class CoverController : Controller
    {
        private readonly IRepo<CoverModel> _coverRepo;

        public CoverController(IRepo<CoverModel> coverRepo)
        {
            _coverRepo = coverRepo;
        }

        [Route("/edit-wedding")]
        public async Task<IActionResult> ChangeWedding(CoverModel model)
        {
            var entities = await _coverRepo.GetAll();
            return View(entities.FirstOrDefault(x=>x.Category == Category.Wedding));
        }
        
        [Route("/edit-civil")]
        public async Task<IActionResult> ChangeCivil(CoverModel model)
        {
            var entities = await _coverRepo.GetAll();
            return View(entities.FirstOrDefault(x => x.Category == Category.CivilWedding));
        }
        
        [Route("/edit-christening")]
        public async Task<IActionResult> ChangeChristening(CoverModel model)
        {
            var entities = await _coverRepo.GetAll();
            return View(entities.FirstOrDefault(x => x.Category == Category.Christening));
        }

        [HttpPost("/edited")]
        public async Task<IActionResult> Edit(CoverModel model)
        {
            if (model == null) return RedirectToAction("Index", "admin", "Admin");
            await _coverRepo.UpdateAsync(model);
            return RedirectToAction("Index", "admin", "Admin");
        }

    }
}
