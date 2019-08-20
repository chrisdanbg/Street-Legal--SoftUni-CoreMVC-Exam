using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository userRepository;
        private readonly ICarRepository carRepository;

        public DriverRepository(ApplicationDbContext context, IUserRepository userRepository, ICarRepository carRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.carRepository = carRepository;
        }

        public async Task<Car> AssignBasicCar()
        {
            return this.carRepository.GetStartingCar();
        }

        public async Task<AssignedVM> SetupProfile(ApplicationUser user)
        {
            if (user.Driver != null)
            {
                return null;
            }
            var firstCarToAssign = await AssignBasicCar();

            Garage userGarage = new Garage()
            {
                Cars = new List<Car>() { firstCarToAssign },
                UserId = user.Id
            };


            user.Driver = new Driver()
            {
                MainCar = firstCarToAssign,
                Experience = 0,
                Garage = userGarage,
                Level = 1,
                Money = 0
            };

            this.context.Update(user);

            if (await this.context.SaveChangesAsync() > 0)
            {
                var AssignedVMToReturn = new AssignedVM()
                {
                    Car = firstCarToAssign,
                    Driver = user.Driver
                };

                return AssignedVMToReturn;
            }

            return null;
        }
    }
}