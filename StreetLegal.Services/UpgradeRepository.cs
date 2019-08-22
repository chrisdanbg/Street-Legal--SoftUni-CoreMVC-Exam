using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.UpgradeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services
{
    public class UpgradeRepository : IUpgradeRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository userRepository;

        public UpgradeRepository(ApplicationDbContext context, IUserRepository userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }

        public async Task<CarPartsVM> GetAvailablePartsToUpgrade(ApplicationUser currentUser, int id)
        {
            var car = await this.context.Cars.Where(c => c.Id == id).Include(c => c.Parts).FirstOrDefaultAsync();

            var driver = this.userRepository.GetDriverProfile(currentUser).Driver;

            List<Part> availableParts = driver.Parts.Where(dp => !car.Parts.Any(cp => cp.Id == dp.Id)).ToList();

            return new CarPartsVM()
            {
                CarId = id,
                PartsOwned = car.Parts,
                PartsAvailable = availableParts
            };
        }

        public async Task<PartsIndexVM> GetPartsIndexVM(ApplicationUser user)
        {
            var driver = this.userRepository.GetDriverProfile(user).Driver;

            List<Part> availableParts = this.context.Parts.Where(c => !driver.Parts.Any(uc => uc.Id == c.Id)).ToList();

            return new PartsIndexVM()
            {
                PartsOwned = driver.Parts,
                PartsAvailable = availableParts
            }; 
        }

        public async Task<bool> InstallPart(ApplicationUser currentUser, int id, int partid)
        {
            var car = await this.context.Cars.Where(c => c.Id == id).Include(c => c.Parts).FirstOrDefaultAsync();
           
            var driver = this.userRepository.GetDriverProfile(currentUser).Driver;

            var part = driver.Parts.FirstOrDefault(p => p.Id == partid);

            car.Parts.Add(part);
            driver.Parts.Remove(part);

            currentUser.Driver = driver;
            this.context.Update(currentUser);
            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PurchasePart(ApplicationUser currentUser, int id)
        {
            var driver = this.userRepository.GetDriverProfile(currentUser).Driver;

            var part = this.context.Parts.FirstOrDefault(p => p.Id == id);

            if (driver.Money < part.Price)
            {
                return false;
            }

            driver.Money -= part.Price;

            driver.Parts.Add(part);

            currentUser.Driver = driver;
            this.context.Update(currentUser);
            
            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
