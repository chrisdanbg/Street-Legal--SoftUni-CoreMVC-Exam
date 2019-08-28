using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Services
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;

        public CarRepository(ApplicationDbContext context, IMapper mapper, IPhotoRepository photoRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
        }

        public async Task<bool> CreateNewCar(CreateCarVM createCarVM)
        {
            var carToCreate = this.mapper.Map<Car>(createCarVM);
            IFormFile carImage = createCarVM.Image;

            Engine engineToAdd = this.context.Engines.FirstOrDefault(e => e.Id == createCarVM.EngineId);
            Tyres tyresToAdd = this.context.Tyres.FirstOrDefault(t => t.Id == createCarVM.TyresId);

            if (tyresToAdd == null || engineToAdd == null) { return false; }

            if (!FileIsValidImage(carImage))
            {
                return false;
            }

            var imageUrl = await this.photoRepository.UploadImageToStorage(carImage);

            if (imageUrl == null)
            {
                return false;
            }

            carToCreate.ImageUrl = imageUrl;
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
                Tyres = GetAllTyres(),
                Parts = GetAllParts()
            };
        }

        private ICollection<Part> GetAllParts()
        {
            return this.context.Parts.ToList();
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
            return await this.context.Cars.Include(c => c.Engine).Include(c => c.Tyres).Include(c => c.Parts).FirstOrDefaultAsync(c => c.Id == carId);
        }

        public async Task<Car> GetCarByMaxSpeed(int rivalMaxSpeed)
        {
            var car = await this.context.Cars.Include(c => c.Engine).Include(c => c.Tyres).Where(c => c.Engine.MaxSpeed <= rivalMaxSpeed).FirstOrDefaultAsync();
            return car;
        }

        public async Task<bool> CreateNewPart(CreatePartVM createPartVM)
        {
            var partToCreate = this.mapper.Map<Part>(createPartVM);
            IFormFile partImage = createPartVM.Image;

            if (!FileIsValidImage(partImage))
            {
                return false;
            }

            var imageUrl = await this.photoRepository.UploadImageToStorage(partImage);

            if (imageUrl == null)
            {
                return false;
            }

            partToCreate.ImageUrl = imageUrl;

            await this.context.AddAsync(partToCreate);

            if (await this.context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return true;
        }

        private bool FileIsValidImage(IFormFile postedFile)
        {
            const int ImageMinimumBytes = 512;

            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(Path.GetExtension(postedFile.FileName), ".jpg"
                                            , StringComparison.OrdinalIgnoreCase)
                && !string.Equals(Path.GetExtension(postedFile.FileName), ".png"
                                            , StringComparison.OrdinalIgnoreCase)
                && !string.Equals(Path.GetExtension(postedFile.FileName), ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                using (var bitmap = new Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        }
    }
}
