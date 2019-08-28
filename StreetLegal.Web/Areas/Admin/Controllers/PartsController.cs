using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Parts")]
    [Authorize(Roles = "Admin")]
    public class PartsController : Controller
    {
        private readonly ICarRepository carRepository;

        public PartsController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreatePartVM createPartVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createPartVM);
            }

            if (await carRepository.CreateNewPart(createPartVM))
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }

            return View(createPartVM);
        }
    }
}