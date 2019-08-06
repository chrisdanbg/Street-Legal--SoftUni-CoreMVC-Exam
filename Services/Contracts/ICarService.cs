using System;
using System.Threading.Tasks;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface ICarService
    {
        Task<bool> CreateNewCar(CreateCarVM createCarVM);
    }
}
