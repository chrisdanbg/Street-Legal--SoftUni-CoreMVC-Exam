using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StreetLegal.Tests.Service
{
    public class GarageRepository_Tests
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
        private readonly ICarRepository carRepository;

        private readonly MapperConfiguration config;

        public GarageRepository_Tests()
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
            this.carRepository = new CarRepository(this.context, mapper);
            this.driverRepository = new DriverRepository(this.context, this.userRepository, this.carRepository);
        }

        [Fact]
        public async void GetCarsForUser_Should_Return_ICollection()
        {
            var expected = typeof(List<Car>);


            ApplicationUser user = new ApplicationUser();

            await this.context.AddAsync(user);

            await this.context.SaveChangesAsync();

            

            var currentUser = await this.context.Users.FirstOrDefaultAsync();


            var garage = new Garage()
            {
                Cars = new List<Car>(),
                UserId = currentUser.Id

            };

            var carToCreate = await CreateCar();

            await this.carRepository.CreateNewCar(carToCreate);

            var car = await this.context.Cars.FirstOrDefaultAsync();

            garage.Cars.Add(car);
                
              await this.context.AddAsync(garage);

            await this.context.SaveChangesAsync();

            var actual = await this.garageRepository.GetCarsForUser(currentUser);

            Assert.IsType(expected, actual);
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
    }
}
