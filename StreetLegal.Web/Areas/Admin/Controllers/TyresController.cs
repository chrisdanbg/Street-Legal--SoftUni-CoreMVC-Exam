using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Tyres")]
    [Authorize(Roles = "Admin")]
    public class TyresController : Controller
    {
        private readonly ICarRepository carRepository;

        public TyresController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateTyresVM createTyres)
        {
            if (!ModelState.IsValid)
            {
                return View(createTyres);
            }

            if (await this.carRepository.CreateNewTyres(createTyres))
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }

            return View(createTyres);
        }
    }
}