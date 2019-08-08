using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class AdminIndexVM
    {
        public ICollection<Engine> Engines { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Tyres> Tyres { get; set; }
    }
}
