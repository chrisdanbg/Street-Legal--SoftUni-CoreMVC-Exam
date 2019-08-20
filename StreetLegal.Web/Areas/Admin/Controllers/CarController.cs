using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Car")]
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly ICarRepository carRepository;

        public CarController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            if (ViewBag.Engines == null)
            {
                ViewBag.Engines = new SelectList(this.carRepository.GetAllEngines(),
                                                nameof(Engine.Id), nameof(Engine.Name), null);
            }

            if (ViewBag.Tyres == null)
            {
                ViewBag.Tyres = new SelectList(this.carRepository.GetAllTyres(),
                                                nameof(Tyres.Id), nameof(Tyres.Name), null);
            }
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCarVM createCarVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createCarVM);
            }

            if (await carRepository.CreateNewCar(createCarVM))
            {
                return RedirectToAction(nameof(AdminController.Index),"Admin");
            }

            return View(createCarVM);
        }
    }
}