using System;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;

namespace StreetLegal.Services
{
    public class DriverRepository : IDriverRepository
    {
        public void AssignBasicCar(int userId)
        {
            Driver userToAssign = userRepository.GetUserById(userId);

            userToAssign.MainCar = GetStartingCar();
        }

        private Car GetStartingCar()
        {
            Tyres startingTyres = new Tyres()
            {
                Health = 100
            };

            return new Car()
            {
                Make = "Honda Civic",
                Year = 1995,
                Tyres = startingTyres,
                Engine = new Engine()
                {
                    HP = 110,
                    MaxSpeed = 180,
                    Name = "Fabric Honda"
                }
            };
        }
    }
}