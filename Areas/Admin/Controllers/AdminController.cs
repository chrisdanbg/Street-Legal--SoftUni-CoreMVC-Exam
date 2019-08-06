using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICarService carService;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICarService carService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.carService = carService;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> Create(CreateCarVM createCarVM)
        {
            if (ModelState.IsValid)
            {
                return View(createCarVM);
            }

            if (await carService.CreateNewCar(createCarVM))
            {
                return RedirectToAction(nameof(AdminController.Index));
            }

            return View(createCarVM);
        }
    }
}
