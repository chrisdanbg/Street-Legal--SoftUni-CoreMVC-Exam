using Microsoft.AspNetCore.Identity;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services
{
    public class RaceRepository : IRaceRepository
    {
        private readonly IUserRepository userRepository;
        private readonly ICarService carService;

        public RaceRepository(IUserRepository userRepository, 
                             ICarService carService)
        {
            this.userRepository = userRepository;
            this.carService = carService;
        }
        public async Task<bool> Race(ApplicationUser user)
        {
            var currentDriver = this.userRepository.GetDriverProfile(user);
            int currentUserCarId = currentDriver.Driver.MainCar.Id;

            Car userCar = await this.carService.GetCarById(currentUserCarId);
            if (userCar == null)
            {
                return false;
            }

            Driver rival = await GenerateRival(currentDriver.Driver, userCar);
            if (rival == null)
            {
                return false;
            }

            return RaceDrivers(currentDriver.Driver, rival);
        }

        private bool RaceDrivers(Driver driver, Driver rival)
        {
            Random rnd = new Random();
            int rivalLuckyNumber = rnd.Next(7);
            int userLuckyNumber = rnd.Next(7);

            int userStats = driver.Level + driver.MainCar.Engine.HP 
                            + driver.MainCar.Engine.MaxSpeed 
                            + driver.MainCar.Tyres.Health 
                            + userLuckyNumber;

            int rivalStats = rival.Level + rival.MainCar.Engine.HP
                            + rival.MainCar.Engine.MaxSpeed
                            + rival.MainCar.Tyres.Health
                            + rivalLuckyNumber;

            return userStats > rivalStats;
        }

        private async Task<Driver> GenerateRival(Driver userDriver, Car userCar)
        {
            int currentUserLevel = userDriver.Level;

            var userCarMaxSpeed = userCar.Engine.MaxSpeed;
            var userCarHP = userCar.Engine.HP;
            var userCarTyresHealth = userCar.Tyres.Health;


            Random rnd = new Random();
            var rivalLevel = rnd.Next(currentUserLevel);
            var rivalMaxSpeed = rnd.Next(userCarMaxSpeed + 20);

            Car rivalCar = await this.carService.GetCarByMaxSpeed(rivalMaxSpeed);

            rivalCar.Tyres.Health = rnd.Next(userDriver.MainCar.Tyres.Health + 20);

            return new Driver()
            {
                MainCar = rivalCar,
                Level = rivalLevel,
            };
        }
    }
}
