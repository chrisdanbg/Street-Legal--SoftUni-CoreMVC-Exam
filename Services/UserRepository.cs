using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;
using StreetLegal.ViewModels.HomeViewModels;

namespace StreetLegal.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public HomeIndexVM GetDriverProfile(ApplicationUser user)
        {
            var userToGetDriverFrom = this.context.Users.Include(u => u.Driver).ThenInclude(d => d.Garage)
                .Include(u => u.Driver)
                .ThenInclude(d => d.MainCar)
                .FirstOrDefault(u => u.Id == user.Id);

            Driver driverToReturn = userToGetDriverFrom.Driver;

            return new HomeIndexVM()
            {
                Driver = driverToReturn
            };
        }

        public Driver GetDriverById(int userId)
        {
            return this.context.Drivers.FirstOrDefault(d => d.Id == userId);
        }
    }
}
