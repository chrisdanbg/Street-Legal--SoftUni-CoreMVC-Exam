using StreetLegal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.UpgradeViewModels
{
    public class PartsIndexVM
    {
        public ICollection<Part> PartsOwned { get; set; }

        public ICollection<Part> PartsAvailable { get; set; }
    }
}
