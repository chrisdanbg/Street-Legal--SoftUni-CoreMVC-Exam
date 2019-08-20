using System.Collections.Generic;
using System.Threading.Tasks;
using StreetLegal.Models.CarModels;
using StreetLegal.ViewModels.AdminViewModels;

namespace StreetLegal.Services.Contracts
{
    public interface ICarRepository
    {
        Task<bool> CreateNewCar(CreateCarVM createCarVM);
        Task<bool> CreateNewEngine(CreateEngineVM createEngine);
        AdminIndexVM PopulateAdminIndexVM();
        ICollection<Engine> GetAllEngines();
        Task<bool> CreateNewTyres(CreateTyresVM createTyres);
        ICollection<Tyres> GetAllTyres();
        Car GetStartingCar();
        Task<Car> GetCarById(int carId);
        Task<Car> GetCarByMaxSpeed(int rivalMaxSpeed);
        Task<bool> CreateNewPart(CreatePartVM createPartVM);
    }
}
