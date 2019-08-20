using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICarRepository carRepository;

        public AdminController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Index()
        {
            AdminIndexVM adminIndexVM = this.carRepository.PopulateAdminIndexVM();
            return View(adminIndexVM);
        }
     
    }
}
