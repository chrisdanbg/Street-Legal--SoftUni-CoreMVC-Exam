using System;
using StreetLegal.Models;

namespace StreetLegal.Services.Contracts
{
    public interface IUserRepository
    {
        Driver GetUserById(int userId);
    }
}
