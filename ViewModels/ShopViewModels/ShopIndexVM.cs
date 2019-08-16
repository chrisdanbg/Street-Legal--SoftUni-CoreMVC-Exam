using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.ShopViewModels
{
    public class ShopIndexVM
    {
        public PurchasableCarsVM CarsToPurchase { get; set; }

        public PurchasedCarsVM CarsOwned { get; set; }
    }
}
