using StreetLegal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services.Contracts
{
    public interface IRaceRepository
    {
        Task<bool> Race(ApplicationUser user);
    }
}
