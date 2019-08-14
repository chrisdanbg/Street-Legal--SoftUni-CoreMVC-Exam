using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.GarageViewModels;

namespace StreetLegal.Controllers
{
    public class GarageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGarageRepository garageRepository;

        public GarageController(UserManager<ApplicationUser> userManager, IGarageRepository garageRepository)
        {
            this.userManager = userManager;
            this.garageRepository = garageRepository;
        }

        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            GarageIndexVM garageIndexVM = await this.garageRepository.GetGarageForUser(currentUser);

            if (garageIndexVM == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(garageIndexVM);
        }
    }
}