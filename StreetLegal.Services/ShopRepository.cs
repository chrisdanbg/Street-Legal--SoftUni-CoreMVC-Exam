using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.ShopViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository userRepository;

        public ShopRepository(ApplicationDbContext context, IUserRepository userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }

        public async Task<PurchasableCarsVM> GetAllPurchasableCars(ApplicationUser currentUser)
        {
            Garage userGarage = await this.context.Garages.Where(g => g.UserId == currentUser.Id).Include(g=> g.Cars).FirstOrDefaultAsync();

            List<Car> availableCars = await this.context.Cars.Where(c => !userGarage.Cars.Any(uc => uc.Id == c.Id)).Include(c => c.Engine).ToListAsync();

            return new PurchasableCarsVM()
            {
                Cars = availableCars
            };
        }

        public async Task<PurchasedCarsVM> GettAllOwnedCars(ApplicationUser currentUser)
        {
            Garage userGarage = await this.context.Garages.Where(g => g.UserId == currentUser.Id).Include(g => g.Cars).ThenInclude(c => c.Engine).FirstOrDefaultAsync();

            return new PurchasedCarsVM()
            {
                OwnedCars = userGarage.Cars
            };
        }

        public async Task<bool> PurchaseCar(ApplicationUser currentUser, int id)
        {
            Car carToPurchase = await this.context.Cars.Where(c => c.Id == id).Include(c => c.Engine).Include(c => c.Tyres).FirstOrDefaultAsync();

            if (carToPurchase == null)
            {
                return false;
            }
            var driver = this.userRepository.GetDriverProfile(currentUser).Driver;

            if (driver.Money < carToPurchase.Value)
            {
                return false;
            }

            driver.Money -= carToPurchase.Value;

            Garage userGarage = await this.context.Garages.Where(g => g.UserId == currentUser.Id).Include(g => g.Cars).ThenInclude(c => c.Engine).FirstOrDefaultAsync();

            userGarage.Cars.Add(carToPurchase);

            driver.Garage = userGarage;

            currentUser.Driver = driver;

            this.context.Update(currentUser);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SellCar(ApplicationUser currentUser, int id)
        {
            var driver = this.userRepository.GetDriverProfile(currentUser).Driver;

            Garage userGarage = await this.context.Garages.Where(g => g.UserId == currentUser.Id).Include(g => g.Cars).ThenInclude(c => c.Engine).FirstOrDefaultAsync();

            Car carToSell = userGarage.Cars.Where(c => c.Id == id).FirstOrDefault();

            if (carToSell == null)
            {
                return false;
            }

            if (driver.MainCar == carToSell)
            {
                driver.MainCar = null;
            }

            userGarage.Cars.Remove(carToSell);

            driver.Garage = userGarage;

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
