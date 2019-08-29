using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;
using StreetLegal.ViewModels.RaceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StreetLegal.Tests.Service
{
    public class UserRepository_Tests
    {
        private readonly DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
       .UseInMemoryDatabase(databaseName: "Temp_TreeRepo_Database")
       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
       .Options;


        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        private readonly IUserRepository userRepository;
        private readonly IGarageRepository garageRepository;
        private readonly IDriverRepository driverRepository;
        private readonly IRaceRepository raceRepository;
        private readonly ICarRepository carRepository;
        private readonly IUpgradeRepository upgradeRepository;
        private readonly MapperConfiguration config;
        private readonly IPhotoRepository photoRepository;
        public UserRepository_Tests()
        {
            this.config = new MapperConfiguration(opts =>
            {
                opts.CreateMap<CreateEngineVM, Engine>();
                opts.CreateMap<Engine, CreateEngineVM>();

                opts.CreateMap<CreateTyresVM, Tyres>();
                opts.CreateMap<Tyres, CreateTyresVM>();

                opts.CreateMap<CreatePartVM, Part>();
                opts.CreateMap<Part, CreatePartVM>();

                opts.CreateMap<CreateCarVM, Car>();
                opts.CreateMap<Car, CreateCarVM>();
            });

            this.mapper = config.CreateMapper();

            this.mapper = config.CreateMapper();
            this.context = new ApplicationDbContext(options);

            this.garageRepository = new GarageRepository(this.context);
            this.userRepository = new UserRepository(this.context, this.garageRepository);
            this.photoRepository = new PhotoRepository();
            this.carRepository = new CarRepository(this.context, mapper, this.photoRepository);
            this.driverRepository = new DriverRepository(this.context, this.userRepository, this.carRepository);
            this.raceRepository = new RaceRepository(this.userRepository, this.carRepository);
            this.upgradeRepository = new UpgradeRepository(this.context, this.userRepository);
        }

        [Fact]
        public async void Reward_Should_Return_ViewModel()
        {
            var user = await GenerateUser();

            var result = await this.userRepository.RewardUser(user);

            Assert.IsType<RaceWinVM>(result);
        }


        [Fact]
        public async void GetDriverLevel_Should_Return_ShouldReturnInt()
        {
            var user = await GenerateUser();

            var result = this.userRepository.GetDriverLevel(user);

            Assert.IsType<int>(result);
        }

        [Fact]
        public async void HasMainCar_Should_Return_True()
        {
            var user = await GenerateUser();


            var result = this.userRepository.HasMainCar(user);

            Assert.True(result);
        }

        [Fact]
        public async void AssignMainCar_Should_Return_True()
        {
            var user = await GenerateUser();

            var carToCreate2 = await CreateCar2();
            var car = await this.context.Cars.FirstOrDefaultAsync();

            await this.carRepository.CreateNewCar(carToCreate2);

            var car2 = await this.context.Cars.Where(c => c.Id != car.Id).FirstOrDefaultAsync();
            var result = await this.userRepository.AssingnMainCar(user,car2.Id);

            Assert.True(result);
        }

        [Fact]
        public async void GetDriverById_Should_Return_Driver()
        {
            var carToCreate = await CreateCar();

            await this.carRepository.CreateNewCar(carToCreate);

            var car = await this.context.Cars.FirstOrDefaultAsync();

            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10
            };

            await this.context.AddAsync(part);

            await this.context.SaveChangesAsync();



            var garage = new Garage()
            {
                Cars = new List<Car> { car },
                UserId = user.Id
            };


            await this.context.AddAsync(garage);

            await this.context.SaveChangesAsync();

            Driver newDriver = new Driver()
            {
                MainCar = car,
                Garage = garage,
                Experience = 100,
                Level = 1,
                Money = 100000,
            };


            newDriver.Parts = new List<Part>() { part };

            user.Driver = newDriver;
            this.context.Update(user);

            await this.context.SaveChangesAsync();

            var result = this.userRepository.GetDriverById(user.Driver.Id);

            Assert.IsType<Driver>(result);
        }

        private async Task<CreateCarVM> CreateCar()
        {
            CreateEngineVM createEngineVM = new CreateEngineVM()
            {
                HP = 10,
                MaxSpeed = 100,
                Name = "Test"
            };

            var engineCreated = await this.carRepository.CreateNewEngine(createEngineVM);
            Assert.True(engineCreated);

            var engine = await this.context.Engines.FirstOrDefaultAsync();

            CreateTyresVM createTyresVM = new CreateTyresVM()
            {
                Health = 1,
                Name = "test",
                TyreType = Models.Enums.TyreType.Hard
            };

            var tyresCreated = await this.carRepository.CreateNewTyres(createTyresVM);
            Assert.True(tyresCreated);
            var tyre = await this.context.Tyres.FirstOrDefaultAsync();

            CreateCarVM createCarVM = new CreateCarVM()
            {
                EngineId = engine.Id,
                TyresId = tyre.Id,
                Make = "testa",
                Value = 1234,
                Year = 1990
            };

            return createCarVM;
        }
        private async Task<CreateCarVM> CreateCar2()
        {
            CreateEngineVM createEngineVM = new CreateEngineVM()
            {
                HP = 110,
                MaxSpeed = 200,
                Name = "Testa"
            };

            var engineCreated = await this.carRepository.CreateNewEngine(createEngineVM);
            Assert.True(engineCreated);

            var engine = await this.context.Engines.FirstOrDefaultAsync();

            CreateTyresVM createTyresVM = new CreateTyresVM()
            {
                Health = 2,
                Name = "teast",
                TyreType = Models.Enums.TyreType.Hard
            };

            var tyresCreated = await this.carRepository.CreateNewTyres(createTyresVM);
            Assert.True(tyresCreated);
            var tyre = await this.context.Tyres.FirstOrDefaultAsync();

            CreateCarVM createCarVM = new CreateCarVM()
            {
                EngineId = engine.Id,
                TyresId = tyre.Id,
                Make = "testa",
                Value = 14,
                Year = 1900
            };

            return createCarVM;
        }

        private async Task<ApplicationUser> GenerateUser()
        {
            var carToCreate = await CreateCar();

            await this.carRepository.CreateNewCar(carToCreate);
   
            var car = await this.context.Cars.FirstOrDefaultAsync();
          
            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10
            };

            await this.context.AddAsync(part);

            await this.context.SaveChangesAsync();



            var garage = new Garage()
            {
                Cars = new List<Car> { car },
                UserId = user.Id
            };


            await this.context.AddAsync(garage);

            await this.context.SaveChangesAsync();

            Driver newDriver = new Driver()
            {
                MainCar = car,
                Garage = garage,
                Experience = 100,
                Level = 1,
                Money = 100000,
            };


            newDriver.Parts = new List<Part>() { part };

            user.Driver = newDriver;
            this.context.Update(user);

            await this.context.SaveChangesAsync();

            return user;
        }
    }
}
