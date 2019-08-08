using System;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IDriverRepository
    {
        Task<Car> AssignBasicCar();
        Task<AssignedVM> SetupProfile(ApplicationUser user);
    }
}
