using System;
using StreetLegal.Models;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IUserRepository
    {
        Driver GetDriverById(int userId);
        HomeIndexVM GetDriverProfile(ApplicationUser user);
    }
}
