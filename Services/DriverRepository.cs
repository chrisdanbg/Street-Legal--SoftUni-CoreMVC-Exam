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
        private readonly ICarService carService;

        public DriverRepository(ApplicationDbContext context, IUserRepository userRepository, ICarService carService)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.carService = carService;
        }

        public async Task<Car> AssignBasicCar()
        {
            return this.carService.GetStartingCar();
        }

        public async Task<AssignedVM> SetupProfile(ApplicationUser user)
        {
            if (user.Driver != null)
            {
                return null;
            }
            var firstCarToAssign = await AssignBasicCar();

            user.Driver = new Driver()
            {
                MainCar = firstCarToAssign,
                Experience = 0,
                Garage = new List<Car>() { firstCarToAssign },
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