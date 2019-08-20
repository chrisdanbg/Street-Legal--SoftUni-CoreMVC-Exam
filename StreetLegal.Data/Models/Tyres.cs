using StreetLegal.Models.Enums;

namespace StreetLegal.Models.CarModels
{
    public class Tyres
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TyreType tyreType { get; set; }

        public int Health { get; set; }
    }
}