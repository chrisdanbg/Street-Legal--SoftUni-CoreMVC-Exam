using System;
using System.Threading.Tasks;
using AutoMapper;
using StreetLegal.Data;
using StreetLegal.Models.CarModels;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext context;

        public CarService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateNewCar(CreateCarVM createCarVM)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CreateCarVM, Car>();
            });
            IMapper iMapper = config.CreateMapper();
            Car carToCreate = iMapper.Map<CreateCarVM, Car>(createCarVM);

            return true;
        }
    }
}
