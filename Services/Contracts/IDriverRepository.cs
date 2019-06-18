using System;
using StreetLegal.Models.CarModels;

namespace StreetLegal.Services.Contracts
{
    public interface IDriverRepository
    {
        void AssignBasicCar(int userId);

        Car GetStartingCar();
    }
}
