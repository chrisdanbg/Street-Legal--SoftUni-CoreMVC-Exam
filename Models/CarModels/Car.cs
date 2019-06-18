using System;
namespace StreetLegal.Models.CarModels
{
    public class Car
    {
            public int Id { get; set; }

            public string Make { get; set; }

            public int Year { get; set; }

            public Engine Engine { get; set;}

            public Tyres Tyres { get; set; }

    }
}
