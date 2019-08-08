using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICarService carService;

        public AdminController(ICarService carService)
        {
            this.carService = carService;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminIndexVM adminIndexVM = this.carService.PopulateAdminIndexVM();
            return View(adminIndexVM);
        }
     
    }
}
