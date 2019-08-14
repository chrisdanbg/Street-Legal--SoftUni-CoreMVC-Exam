using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;
using StreetLegal.ViewModels.RaceViewModels;

namespace StreetLegal.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public HomeIndexVM GetDriverProfile(ApplicationUser user)
        {
            var userToGetDriverFrom = this.context.Users.Include(u => u.Driver).ThenInclude(d => d.Garage)
                .Include(u => u.Driver)
                .ThenInclude(d => d.MainCar)
                .FirstOrDefault(u => u.Id == user.Id);

            Driver driverToReturn = userToGetDriverFrom.Driver;

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
    }
}
