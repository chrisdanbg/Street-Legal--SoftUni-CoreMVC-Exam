using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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

        public async Task<bool> RewardUser(ApplicationUser applicationUser)
        {
            var driver  = GetDriverProfile(applicationUser).Driver;

            var userLevel = driver.Level;
            var userExperience = driver.Experience;

            var rnd = new Random();

            var awardPoints = 3 * userLevel;
            var awardMoney = rnd.Next(100, 1000) * userLevel;

            if (UserCanLevelUp(applicationUser,awardPoints))
            {
                await LevelUp(applicationUser, awardPoints);
                return true;
            }

            await AwardExperience(applicationUser, awardPoints);
            await AwardMoney(applicationUser, awardMoney);
             
            return false;
        }

        private async Task AwardMoney(ApplicationUser applicationUser, int awardMoney)
        {
            var driver = GetDriverProfile(applicationUser).Driver;
            driver.Money = awardMoney;
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

        private async Task LevelUp(ApplicationUser applicationUser, int awardPoints)
        {
            var driver = GetDriverProfile(applicationUser).Driver;

            driver.Level++;
    
            applicationUser.Driver = driver;

            this.context.Update(applicationUser);

            await this.context.SaveChangesAsync();

        }

        private bool UserCanLevelUp(ApplicationUser applicationUser, int awardPoints)
        {
            var driver = GetDriverProfile(applicationUser).Driver;

            var userLevel = driver.Level;
            var userExperience = driver.Experience;

            int pointsNeededForLevelUp = userLevel * 50;

            return (userExperience + awardPoints) >= pointsNeededForLevelUp;
        }
    }
}
