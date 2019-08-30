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
using StreetLegal.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StreetLegal.Tests.Service
{
    public class DriverRepository_Tests
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
        private readonly IPhotoRepository photoRepository;
        private readonly MapperConfiguration config;

        public DriverRepository_Tests()
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
            this.userRepository = new UserRepository(this.context,this.garageRepository);
            this.photoRepository = new PhotoRepository();
            this.carRepository = new CarRepository(this.context, mapper,this.photoRepository);
            this.driverRepository = new DriverRepository(this.context,this.userRepository,this.carRepository);
        }

        //[Fact]
        //public async void AssignBasicCar_Should_ReturnCarTask()
        //{
        //    var expected = typeof(Car);

        //    var createCarVM = await CreateCar();
        //    var isCreated = await this.carRepository.CreateNewCar(createCarVM);

        //    Assert.True(isCreated);

        //    var result = await this.driverRepository.AssignBasicCar();


        //    Assert.IsType(expected, result);
        //}

        [Fact]
        public async void SetupProfile_ShouldWork()
        {

            ApplicationUser user = new ApplicationUser();

            var createCarVM = await CreateCar();
            await this.carRepository.CreateNewCar(createCarVM);

            this.context.AddAsync(user);
            this.context.SaveChanges();

            var result = await this.driverRepository.SetupProfile(user);

            Assert.IsType<AssignedVM>(result);

        }

        [Fact]
        public async void SetupProfile_ShouldReturnNull_IfNoCarFound()
        {

            ApplicationUser user = new ApplicationUser();

            this.context.AddAsync(user);
            this.context.SaveChanges();


            user.Driver = new Driver();

            var result = await this.driverRepository.SetupProfile(user);

            Assert.Null(result);

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

                Assert.True(isCreated);

                car = createCarVM;
            }

            return car;
        }
    }
}
