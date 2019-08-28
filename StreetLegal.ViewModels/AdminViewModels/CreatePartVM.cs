using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreatePartVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BonusPoints { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
