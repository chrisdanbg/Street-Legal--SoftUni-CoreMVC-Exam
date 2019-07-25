using System;
using StreetLegal.Data;
using StreetLegal.Models;
using StreetLegal.Services.Contracts;

namespace StreetLegal.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Driver GetUserById(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
