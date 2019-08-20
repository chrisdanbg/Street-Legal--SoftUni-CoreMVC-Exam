using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Helpers;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.UpgradeViewModels;

namespace StreetLegal.Controllers
{
    public class UpgradeController : Controller
    {
        private readonly IUpgradeRepository upgradeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ErrorMessageHelper errorMessageHelper;

        public UpgradeController(IUpgradeRepository upgradeRepository, UserManager<ApplicationUser> userManager, ErrorMessageHelper errorMessageHelper)
        {
            this.upgradeRepository = upgradeRepository;
            this.userManager = userManager;
            this.errorMessageHelper = errorMessageHelper;
        }
        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            PartsIndexVM viewModel = await this.upgradeRepository.GetPartsIndexVM(currentUser);

            return View(viewModel);
        }

        public async Task<IActionResult> Car (int id)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            CarPartsVM viewModel = await this.upgradeRepository.GetAvailablePartsToUpgrade(currentUser, id);
            return View(viewModel);
        }

        public async Task<IActionResult> Purchase(int id)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (await this.upgradeRepository.PurchasePart(currentUser, id))
            {
                TempData["Message"] = errorMessageHelper.SaveChangesSuccsessMessage;
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = errorMessageHelper.NotEnoughFundsMessage;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Install(int carId,int partid)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (await this.upgradeRepository.InstallPart(currentUser,carId,partid))
            {
                TempData["Message"] = errorMessageHelper.SaveChangesSuccsessMessage;
                return RedirectToAction(nameof(Index), "Garage");
            }
            TempData["Error"] = errorMessageHelper.SaveChangesErrorMessage;
            return RedirectToAction(nameof(Index), "Garage");
        }
    }
}