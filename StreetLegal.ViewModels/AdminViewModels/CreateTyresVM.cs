using StreetLegal.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreateTyresVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Health { get; set; }

        public TyreType TyreType { get; set; }
    }
}
