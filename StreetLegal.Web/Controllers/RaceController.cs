using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.RaceViewModels;

namespace StreetLegal.Controllers
{
    [Authorize]
    public class RaceController : Controller
    {
        private readonly IRaceRepository raceRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;

        public RaceController(IRaceRepository raceRepository, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            this.raceRepository = raceRepository;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Start()
        {

            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (currentUser == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (!this.userRepository.IsAssigned(currentUser))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if(!this.userRepository.HasMainCar(currentUser))
            {
                return RedirectToAction(nameof(GarageController.Index), "Garage");
            }

            return View();
        }

        public async Task<IActionResult> Finish()
        {
            string userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (currentUser == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (await this.raceRepository.Race(currentUser))
            {
                int currentUserLevel = this.userRepository.GetDriverLevel(currentUser);

                RaceWinVM raceWinVM = await this.userRepository.RewardUser(currentUser);

                if (raceWinVM.Level > currentUserLevel)
                {
                    return RedirectToAction(nameof(LevelUp), raceWinVM);
                }

                return RedirectToAction(nameof(Win), raceWinVM);
            }

            return View();
        }

        public IActionResult LevelUp(RaceWinVM raceWinVM)
        {
            return View(raceWinVM);
        }

        public IActionResult Win(RaceWinVM raceWinVM)
        {
            return View(raceWinVM);
        }
    }
}