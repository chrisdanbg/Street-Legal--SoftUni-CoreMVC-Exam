using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StreetLegal.Models.CarModels;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreateCarVM
    {
        [Required]
        public string Make { get; set; }

        [Required]
        public int Year { get; set; }

        public int EngineId { get; set; }

        public int TyresId { get; set; }

        [Required]
        public int Value { get; set; }

        public IFormFile Image { get; set; }
    }
}
