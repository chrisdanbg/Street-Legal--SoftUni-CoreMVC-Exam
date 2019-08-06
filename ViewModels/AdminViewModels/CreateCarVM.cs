using System;
using System.ComponentModel.DataAnnotations;
using StreetLegal.Models.CarModels;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreateCarVM
    {
        [Required]
        public string Make { get; set; }

        [Required]
        public int Year { get; set; }

        public Engine Engine { get; set; }

        public Tyres Tyres { get; set; }
    }
}
