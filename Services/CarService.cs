using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CarService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> CreateNewCar(CreateCarVM createCarVM)
        {
            var carToCreate = this.mapper.Map<Car>(createCarVM);

            Engine engineToAdd = this.context.Engines.FirstOrDefault(e => e.Id == createCarVM.EngineId);
            Tyres tyresToAdd = this.context.Tyres.FirstOrDefault(t => t.Id == createCarVM.TyresId);

            if (tyresToAdd == null || engineToAdd == null) { return false; }

            carToCreate.Engine = engineToAdd;
            carToCreate.Tyres = tyresToAdd;

            await this.context.AddAsync(carToCreate);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CreateNewEngine(CreateEngineVM createEngine)
        {
            var engineToCreate = this.mapper.Map<Engine>(createEngine);

            await this.context.AddAsync(engineToCreate);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public ICollection<Engine> GetAllEngines()
        {
            return this.context.Engines.ToList();
        }

        public ICollection<Car> GetAllCars()
        {
            return this.context.Cars.ToList();
        }

        public AdminIndexVM PopulateAdminIndexVM()
        {
            return new AdminIndexVM()
            {
                Cars = GetAllCars(),
                Engines = GetAllEngines(),
                Tyres = GetAllTyres()
            };
        }

        public async Task<bool> CreateNewTyres(CreateTyresVM createTyres)
        {
            var tyreToCreate = this.mapper.Map<Tyres>(createTyres);

            await this.context.AddAsync(tyreToCreate);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return true;
        }

        public ICollection<Tyres> GetAllTyres()
        {
            return this.context.Tyres.ToList();

        }

        public Car GetStartingCar()
        {
            var basicCarsToRandomize = this.context.Cars.AsQueryable();

            List<Car> filteredCars = basicCarsToRandomize.Include(c => c.Engine).Include(c => c.Tyres).Where(c => c.Engine.MaxSpeed < 160).ToList();

            if (!filteredCars.Any())
            {
                return null;
            }

            Random randomizer = new Random();

            int r = randomizer.Next(filteredCars.Count);

            return filteredCars[r];
        }

        public async Task<Car> GetCarById(int carId)
        {
            return await this.context.Cars.Include(c => c.Engine).Include(c => c.Tyres).FirstOrDefaultAsync(c => c.Id == carId);
        }

        public async Task<Car> GetCarByMaxSpeed(int rivalMaxSpeed)
        {
            return await this.context.Cars.Where(c => c.Engine.MaxSpeed == rivalMaxSpeed).Include(c => c.Engine).Include(c => c.Tyres).FirstOrDefaultAsync();
        }
    }
}
