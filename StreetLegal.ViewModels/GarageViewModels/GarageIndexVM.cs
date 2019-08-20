using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.GarageViewModels
{
    public class GarageIndexVM
    {
        public int CarsCount { get; set; }

        public ICollection<Car> Cars { get; set; }

    }
}
