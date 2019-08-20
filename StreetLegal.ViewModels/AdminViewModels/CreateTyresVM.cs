using StreetLegal.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreateTyresVM
    { 
        public string Name { get; set; }

        public int Health { get; set; }

        public TyreType TyreType { get; set; }
    }
}
