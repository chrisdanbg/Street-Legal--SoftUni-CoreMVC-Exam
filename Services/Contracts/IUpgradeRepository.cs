using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.ViewModels.UpgradeViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IUpgradeRepository
    {
        Task<PartsIndexVM> GetPartsIndexVM(ApplicationUser user);
        Task<bool> PurchasePart(ApplicationUser currentUser, int id);
        Task<CarPartsVM> GetAvailablePartsToUpgrade(ApplicationUser currentUser, int id);
        Task<bool> InstallPart(ApplicationUser currentUser, int id, int partid);
    }
}
