using System;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IUserRepository
    {
        Driver GetDriverById(int userId);
        HomeIndexVM GetDriverProfile(ApplicationUser user);
        Task<bool> RewardUser(ApplicationUser user);
    }
}
