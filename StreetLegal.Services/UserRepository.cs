﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;
using StreetLegal.ViewModels.RaceViewModels;

namespace StreetLegal.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IGarageRepository garageRepository;

        public UserRepository(ApplicationDbContext context, IGarageRepository garageRepository)
        {
            this.context = context;
            this.garageRepository = garageRepository;
        }

        public HomeIndexVM GetDriverProfile(ApplicationUser user)
        {
            var userToGetDriverFrom = this.context.Users.Include(u => u.Driver).ThenInclude(d => d.Garage)
                .Include(u => u.Driver)
                .ThenInclude(d => d.MainCar)
                .Include(u => u.Driver)
                .ThenInclude(u => u.Parts)
                .FirstOrDefault(u => u.Id == user.Id);

            var cars = this.garageRepository.GetCarsForUser(user).GetAwaiter().GetResult();

            Driver driverToReturn = userToGetDriverFrom.Driver;

            if (cars.Count != 0)
                driverToReturn.Garage.Cars = cars;

            return new HomeIndexVM()
            {
                Driver = driverToReturn
            };
        }

        public Driver GetDriverById(int userId)
        {
            return this.context.Drivers.FirstOrDefault(d => d.Id == userId);
        }

        public async Task<RaceWinVM> RewardUser(ApplicationUser applicationUser)
        {
            Driver driver  = GetDriverProfile(applicationUser).Driver;

            int userLevel = driver.Level;

            Random rnd = new Random();

            int awardPoints = 3 * userLevel;
            int awardMoney = rnd.Next(100, 1000) * userLevel;

            int awardLevel = driver.Level;

            if (UserCanLevelUp(applicationUser,awardPoints))
            {
                awardLevel = await LevelUp(applicationUser);
            }

            await AwardExperience(applicationUser, awardPoints);
            await AwardMoney(applicationUser, awardMoney);



            return new RaceWinVM()
            {
                ExperienceEarned = awardPoints,
                MoneyEarned = awardMoney,
                Level = awardLevel
            };
        }

        private async Task AwardMoney(ApplicationUser applicationUser, int awardMoney)
        {
            var driver = GetDriverProfile(applicationUser).Driver;
            driver.Money += awardMoney;
            applicationUser.Driver = driver;

            this.context.Update(applicationUser);

            await this.context.SaveChangesAsync();
        }

        private async Task AwardExperience(ApplicationUser applicationUser,int awardPoints)
        {
            var driver = GetDriverProfile(applicationUser).Driver;

            driver.Experience += awardPoints;
            applicationUser.Driver = driver;

            this.context.Update(applicationUser);

            await this.context.SaveChangesAsync();
        }

        private async Task<int> LevelUp(ApplicationUser applicationUser)
        {
            var driver = GetDriverProfile(applicationUser).Driver;

            driver.Level++;
    
            applicationUser.Driver = driver;

            this.context.Update(applicationUser);

            await this.context.SaveChangesAsync();

            return driver.Level;

        }

        private bool UserCanLevelUp(ApplicationUser applicationUser, int awardPoints)
        {
            Driver driver = GetDriverProfile(applicationUser).Driver;

            int userLevel = driver.Level;
            int userExperience = driver.Experience;

            int pointsNeededForLevelUp = userLevel * 50;

            return (userExperience + awardPoints) >= pointsNeededForLevelUp;
        }

        public int GetDriverLevel(ApplicationUser applicationUser)
        {
            return GetDriverProfile(applicationUser).Driver.Level;
        }

        public bool HasMainCar(ApplicationUser currentUser)
        {
            return GetDriverProfile(currentUser).Driver.MainCar != null;
        }

        public async Task<bool> AssingnMainCar(ApplicationUser currentUser, int id)
        {
            var carToAssign = this.context.Cars.FirstOrDefault(c => c.Id == id);

            if (carToAssign == null)
            {
                return false;
            }

            var driver = GetDriverProfile(currentUser).Driver;

            driver.MainCar = carToAssign;

            currentUser.Driver = driver;

            this.context.Update(currentUser);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public bool IsAssigned(ApplicationUser currentUser)
        {
            return GetDriverProfile(currentUser).Driver != null;
        }
    }
}
