using System;
using System.Collections;
using System.Collections.Generic;
using StreetLegal.Models.CarModels;

namespace StreetLegal.Models
{
    public class Driver
    {
        public Car MainCar { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        public int Money { get; set; }

        public ICollection<Car> Garage { get; set; }
    }
}
