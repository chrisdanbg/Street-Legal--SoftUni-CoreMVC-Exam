using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;
using StreetLegal.ViewModels.UpgradeViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StreetLegal.Tests.Service
{
    public class UpgradeRepository_Tests
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
        public UpgradeRepository_Tests()
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
        public async void GetAvailableParts_Should_Return_ViewModel()
        {
            var carToCreate = await CreateCar();
            var carToCreate2 = await CreateCar2();

            await this.carRepository.CreateNewCar(carToCreate);
            await this.carRepository.CreateNewCar(carToCreate2);

            var car = await this.context.Cars.FirstOrDefaultAsync();
            var car2 = this.context.Cars.Where(c => c.Id != car.Id).FirstOrDefault();

            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10,
                ImageUrl = "testUrl"
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

            var result = await this.upgradeRepository.GetAvailablePartsToUpgrade(user, car.Id);

            Assert.IsType<CarPartsVM>(result);
        }

        [Fact]
        public async void GetPartsIndexVM_Should_Return_ViewModel()
        {
            var carToCreate = await CreateCar();
            var carToCreate2 = await CreateCar2();

            await this.carRepository.CreateNewCar(carToCreate);
            await this.carRepository.CreateNewCar(carToCreate2);

            var car = await this.context.Cars.FirstOrDefaultAsync();
            var car2 = this.context.Cars.Where(c => c.Id != car.Id).FirstOrDefault();

            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10,
                ImageUrl = "testUrl"
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

            var result = await this.upgradeRepository.GetPartsIndexVM(user);

            Assert.IsType<PartsIndexVM>(result);
        }

        [Fact]
        public async void InstallParts_Should_Return_True()
        {
            var carToCreate = await CreateCar();
            var carToCreate2 = await CreateCar2();

            await this.carRepository.CreateNewCar(carToCreate);
            await this.carRepository.CreateNewCar(carToCreate2);

            var car = await this.context.Cars.FirstOrDefaultAsync();
            var car2 = this.context.Cars.Where(c => c.Id != car.Id).FirstOrDefault();

            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10,
                ImageUrl = "testUrl"
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

            var result = await this.upgradeRepository.InstallPart(user, car.Id,part.Id);

            Assert.True(result);
        }

        [Fact]
        public async void PurchaseParts_Should_Return_True()
        {
            var carToCreate = await CreateCar();
            var carToCreate2 = await CreateCar2();

            await this.carRepository.CreateNewCar(carToCreate);
            await this.carRepository.CreateNewCar(carToCreate2);

            var car = await this.context.Cars.FirstOrDefaultAsync();
            var car2 = this.context.Cars.Where(c => c.Id != car.Id).FirstOrDefault();

            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            var part = new Part()
            {
                BonusPoints = 100,
                Name = "test",
                Price = 10,
                ImageUrl = "testUrl"
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


            newDriver.Parts = new List<Part>();

            user.Driver = newDriver;
            this.context.Update(user);

            await this.context.SaveChangesAsync();

            var result = await this.upgradeRepository.PurchasePart(user, part.Id);

            Assert.True(result);
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

            CreateCarVM car = new CreateCarVM();

            using (var stream = File.OpenRead(@"../../../image.jpg"))
            {
                var file = new FormFile(stream, 0, stream.Length, "Name", Path.GetFileName(@"../../../image.jpg"))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };


                CreateCarVM createCarVM = new CreateCarVM()
                {
                    EngineId = engine.Id,
                    TyresId = tyre.Id,
                    Make = "testa",
                    Value = 1234,
                    Year = 1990,
                    Image = file
                };

                var isCreated = await this.carRepository.CreateNewCar(createCarVM);

                car = createCarVM;
            }

            return car;
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

            CreateCarVM car = new CreateCarVM();

            using (var stream = File.OpenRead(@"../../../image.jpg"))
            {
                var file = new FormFile(stream, 0, stream.Length, "Name", Path.GetFileName(@"../../../image.jpg"))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };


                CreateCarVM createCarVM = new CreateCarVM()
                {
                    EngineId = engine.Id,
                    TyresId = tyre.Id,
                    Make = "testa",
                    Value = 1234,
                    Year = 1990,
                    Image = file
                };

                var isCreated = await this.carRepository.CreateNewCar(createCarVM);

                car = createCarVM;
            }

            return car;
        }
    }
}
