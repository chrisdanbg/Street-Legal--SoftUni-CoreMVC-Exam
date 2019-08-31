using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Helpers;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.ShopViewModels;

namespace StreetLegal.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopRepository shopRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ErrorMessageHelper errorMessageHelper;
        private readonly IUserRepository userRepository;

        public ShopController(IShopRepository shopRepository, UserManager<ApplicationUser> userManager, ErrorMessageHelper errorMessageHelper, IUserRepository userRepository)
        {
            this.shopRepository = shopRepository;
            this.userManager = userManager;
            this.errorMessageHelper = errorMessageHelper;
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (!this.userRepository.IsAssigned(currentUser))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            PurchasableCarsVM carsToPurchase = await this.shopRepository.GetAllPurchasableCars(currentUser);
            PurchasedCarsVM carsOwned = await this.shopRepository.GettAllOwnedCars(currentUser);

            ShopIndexVM shopIndexVM = new ShopIndexVM()
            {
                CarsOwned = carsOwned,
                CarsToPurchase = carsToPurchase
            };

            return View(shopIndexVM);
        }

        public async Task<IActionResult> Purchase (int id)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (await this.shopRepository.PurchaseCar(currentUser,id))
            {
                TempData["Message"] = errorMessageHelper.PurchaseCompletedMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = errorMessageHelper.NotEnoughFundsMessage;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sell (int id)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (await this.shopRepository.SellCar(currentUser, id))
            {
                TempData["Message"] = errorMessageHelper.SoldMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = errorMessageHelper.SaveChangesErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }
}