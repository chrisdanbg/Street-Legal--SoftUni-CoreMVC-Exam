using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.GarageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services
{
    public class GarageRepository : IGarageRepository
    {
        private readonly ApplicationDbContext context;

        public GarageRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Car>> GetCarsForUser(ApplicationUser user)
        {
            var garage = await GetGarageForUser(user);

            return garage.Cars;
        }

        public async Task<GarageIndexVM> GetGarageForUser(ApplicationUser currentUser)
        {

            Garage currentGarage = await this.context.Garages.Where(g => g.UserId == currentUser.Id).Include(d => d.Cars).FirstOrDefaultAsync(); ;

            List<Car> cars = new List<Car>();

            foreach (var car in currentGarage.Cars)
            {
                var carToAdd = await this.context.Cars.Where(c => c.Id == car.Id).Include(c => c.Engine).FirstOrDefaultAsync();

                if (carToAdd == null)
                {
                    continue;
                }

                cars.Add(carToAdd);
            }

            return new GarageIndexVM()
            {
                Cars = cars,
                CarsCount = cars.Count
            };
        }
    }
}
