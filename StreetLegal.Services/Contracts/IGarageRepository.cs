using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.ViewModels.GarageViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IGarageRepository
    {
        Task<GarageIndexVM> GetGarageForUser(ApplicationUser currentUser);
        Task<ICollection<Car>> GetCarsForUser(ApplicationUser user);
    }
}
