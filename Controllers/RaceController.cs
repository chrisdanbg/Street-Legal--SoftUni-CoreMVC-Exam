using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;

namespace StreetLegal.Controllers
{
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
        public IActionResult Start()
        {
            return View();
        }

        public async Task<IActionResult> Finish()
        {
            var currentUser = await this.userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (await this.raceRepository.Race(currentUser))
            {
                if (await this.userRepository.RewardUser(currentUser))
                {
                    return View("Win");
                }
            }

            return View();
        }
    }
}