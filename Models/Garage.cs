using StreetLegal.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public ICollection<Car> Cars { get; set; }

        public string UserId { get; set; }

    }
}
