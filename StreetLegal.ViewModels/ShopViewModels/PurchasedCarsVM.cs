using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.ShopViewModels
{
    public class PurchasedCarsVM
    {
        public ICollection<Car> OwnedCars { get; set; }
    }
}
