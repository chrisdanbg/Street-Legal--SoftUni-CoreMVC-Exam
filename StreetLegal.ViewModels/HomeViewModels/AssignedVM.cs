using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.HomeViewModels
{
    public class AssignedVM
    {
        public Car Car { get; set; }

        public Driver Driver { get; set; }
    }
}
