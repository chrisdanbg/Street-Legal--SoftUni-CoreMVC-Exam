﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace StreetLegal.Tests.Service
{
    public class CarRepository_Tests
    {
        private readonly DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "Temp_TreeRepo_Database")
           .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
           .Options;


        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        private readonly ICarRepository carRepository;

        private readonly IPhotoRepository photoRepository;

        private readonly MapperConfiguration config;

        public CarRepository_Tests()
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
            this.context = new ApplicationDbContext(options);
            this.photoRepository = new PhotoRepository();
            this.carRepository = new CarRepository(this.context, mapper, this.photoRepository);
        }
        [Fact]
        public async void CreateNewEngine_Should_Create()
        {
            
            CreateEngineVM createEngineVM = new CreateEngineVM()
            {
                HP = 10,
                MaxSpeed = 100,
                Name = "Test"
            };

            var isCreated = await this.carRepository.CreateNewEngine(createEngineVM);

            Assert.True(isCreated);
        }

        [Fact]
        public async void CreateNewTyres_Should_Create()
        {
            CreateTyresVM createTyresVM = new CreateTyresVM()
            {
               Health = 1,
               Name = "test",
               TyreType = Models.Enums.TyreType.Hard
            };

            var isCreated = await this.carRepository.CreateNewTyres(createTyresVM);

            Assert.True(isCreated);
        }

        [Fact]
        public async void CreateNewPart_Should_Create()
        {


            using (var stream = File.OpenRead(@"../../../image.jpg"))
            {
                var file = new FormFile(stream, 0, stream.Length, "Name", Path.GetFileName(@"../../../image.jpg"))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };



                CreatePartVM createPartVM = new CreatePartVM()
                {
                    BonusPoints = 100,
                    Price = 2,
                    Image = file
                };

                var isCreated = await this.carRepository.CreateNewPart(createPartVM);

                Assert.True(isCreated);

            }
        }

        [Fact]
        public async void CreateNewCar_Should_Create()
        {
            var expected = await this.context.Cars.CountAsync();

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

            }
        }

        [Fact]
        public async void CreateNewCar_Should_FailIfNoEngineOrTyreProvided()
        {
            var expected = await this.context.Cars.CountAsync();

           
            CreateCarVM createCarVM = new CreateCarVM()
            {
                EngineId = 123,
                TyresId = 13,
                Make = "testa",
                Value = 1234,
                Year = 1990
            };

            var isCreated = await this.carRepository.CreateNewCar(createCarVM);
            var actual = await this.context.Cars.CountAsync();

            Assert.False(isCreated);
        }

        [Fact]
        public async void GetAllEngines_Should_Return_AllEngines()
        {

            var expected = typeof(List<Engine>);

            var result = this.carRepository.GetAllEngines();

            Assert.IsType(expected, result);
        }

        [Fact]
        public async void GetAllCars_Should_Return_AllCars()
        {

            var expected = typeof(List<Car>);

            var result = this.carRepository.GetAllCars();

            Assert.IsType(expected, result);
        }

        [Fact]
        public async void PopulateAdminIndexVM_Should_Populate()
        {

            var expected = typeof(AdminIndexVM);

            var result = this.carRepository.PopulateAdminIndexVM();

            Assert.IsType(expected, result);
        }

        [Fact]
        public async void GetStartingCar_Should_ReturnCar()
        {
            var expected = await this.context.Cars.CountAsync();

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

            }

            var result =  this.carRepository.GetStartingCar();

            Assert.IsType<Car>(result);
        }

        [Fact]
        public async void GetCarByID_Should_ReturnCar()
        {
            var expected = await this.context.Cars.CountAsync();

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

            }

            var car = await this.context.Cars.FirstOrDefaultAsync();

            var result = await this.carRepository.GetCarById(car.Id);

            Assert.IsType<Car>(result);
        }

        [Fact]
        public async void GetCarByMaxSpeed_Should_ReturnCar()
        {
            var expected = await this.context.Cars.CountAsync();

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

            }

            var result = await this.carRepository.GetCarByMaxSpeed(200);

            Assert.IsType<Car>(result);
        }
    }
}
