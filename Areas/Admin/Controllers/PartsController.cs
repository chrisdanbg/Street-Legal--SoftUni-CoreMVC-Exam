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
    [Route("Parts")]
    [Authorize(Roles = "Admin")]
    public class PartsController : Controller
    {
        private readonly ICarService carService;

        public PartsController(ICarService carService)
        {
            this.carService = carService;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
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

            if (await carService.CreateNewPart(createPartVM))
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }

            return View(createPartVM);
        }
    }
}