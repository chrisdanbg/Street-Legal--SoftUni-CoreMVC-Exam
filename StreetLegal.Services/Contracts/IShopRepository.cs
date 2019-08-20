using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreetLegal.Models;
using StreetLegal.ViewModels.ShopViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface IShopRepository
    {
        Task<PurchasableCarsVM> GetAllPurchasableCars(ApplicationUser currentUser);
        Task<PurchasedCarsVM> GettAllOwnedCars(ApplicationUser currentUser);
        Task<bool> PurchaseCar(ApplicationUser currentUser, int id);
        Task<bool> SellCar(ApplicationUser currentUser, int id);
    }
}
