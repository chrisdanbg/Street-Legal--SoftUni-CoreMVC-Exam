using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.ShopViewModels
{
    public class PurchasableCarsVM
    {
        public ICollection<Car> Cars { get; set; }
    }
}
