using System.Collections.Generic;
using System.Threading.Tasks;
using AutomobiliuNuoma;

namespace AutomobiliuNuoma.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<IEnumerable<OilFuelCar>> GetOilFuelCarsAsync();
        Task<IEnumerable<ElectricCar>> GetElectricCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}