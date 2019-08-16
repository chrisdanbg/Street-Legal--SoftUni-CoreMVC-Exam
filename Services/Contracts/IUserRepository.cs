using System;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.ViewModels.HomeViewModels;
using StreetLegal.ViewModels.RaceViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IUserRepository
    {
        Driver GetDriverById(int userId);
        HomeIndexVM GetDriverProfile(ApplicationUser user);
        Task<RaceWinVM> RewardUser(ApplicationUser user);
        int GetDriverLevel(ApplicationUser applicationUser);
        bool HasMainCar(ApplicationUser currentUser);
    }
}
