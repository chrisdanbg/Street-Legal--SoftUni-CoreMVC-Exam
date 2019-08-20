using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
