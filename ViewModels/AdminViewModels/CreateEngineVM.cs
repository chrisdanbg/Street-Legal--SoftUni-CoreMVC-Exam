using System;
using System.ComponentModel.DataAnnotations;

namespace StreetLegal.ViewModels.AdminViewModels
{
    public class CreateEngineVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int HP { get; set; }

        [Required]
        public int MaxSpeed { get; set; }
    }
}
