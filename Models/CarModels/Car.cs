using System;
using System.Collections.Generic;

namespace StreetLegal.Models.CarModels
{
    public class Car
    {
            public int Id { get; set; }

            public string Make { get; set; }

            public int Year { get; set; }

            public Engine Engine { get; set;}

            public Tyres Tyres { get; set; }

            public int Value { get; set; }
            
            public ICollection<Part> Parts { get; set; }

    }
}
