using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Helpers;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.GarageViewModels;

namespace StreetLegal.Controllers
{
    [Authorize]
    public class GarageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGarageRepository garageRepository;
        private readonly IUserRepository userRepository;
        private readonly ErrorMessageHelper errorMessageHelper;

        public GarageController(UserManager<ApplicationUser> userManager, IGarageRepository garageRepository, IUserRepository userRepository, ErrorMessageHelper errorMessageHelper)
        {
            this.userManager = userManager;
            this.garageRepository = garageRepository;
            this.userRepository = userRepository;
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

            if (!this.userRepository.IsAssigned(currentUser))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            GarageIndexVM garageIndexVM = await this.garageRepository.GetGarageForUser(currentUser);

            if (garageIndexVM == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(garageIndexVM);
        }

        public async Task<IActionResult> Main(int id)
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (await this.userRepository.AssingnMainCar(currentUser, id))
            {
                TempData["Message"] = errorMessageHelper.SaveChangesSuccsessMessage;
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = errorMessageHelper.SaveChangesErrorMessage;
            return RedirectToAction(nameof(Index));

        }
    }
}