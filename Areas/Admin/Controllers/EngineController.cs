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
    [Route("Engine")]
    [Authorize(Roles = "Admin")]
    public class EngineController : Controller
    {
        private readonly ICarService carService;

        public EngineController(ICarService carService)
        {
            this.carService = carService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateEngineVM createEngine)
        {
            if (!ModelState.IsValid)
            {
                return View(createEngine);
            }

            if (await this.carService.CreateNewEngine(createEngine))
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }

            return View(createEngine);
        }
    }
}