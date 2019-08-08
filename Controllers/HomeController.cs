using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IDriverRepository driverRepository;

        public HomeController(UserManager<ApplicationUser> userManager, IUserRepository userRepository, IDriverRepository driverRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.driverRepository = driverRepository;
        }

   
        public async Task<IActionResult> Index()
        {
                var user = await this.userManager.GetUserAsync(User);

                HomeIndexVM getDiverProfile = this.userRepository.GetDriverProfile(user);

                return View(getDiverProfile);
        }

        public async Task<IActionResult> Assign()
        {
            var user = await this.userManager.GetUserAsync(User);

            AssignedVM asignedInfo = await this.driverRepository.SetupProfile(user);

            if (asignedInfo != null)
            {
                return View("Assigned", asignedInfo);
            }

            return RedirectToAction(nameof(HomeController.Index));
        }

        public async Task<IActionResult> Assigned(AssignedVM assignedVM)
        {

            var user = await this.userManager.GetUserAsync(User);
            if (user.Driver != null)
            {
                return View(assignedVM);
            }
            return RedirectToAction(nameof(HomeController.Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
