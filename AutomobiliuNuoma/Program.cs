using System.Threading.Tasks;
using AutomobiliuNuoma.Repositories;
using AutomobiliuNuoma.Services;
using AutomobiliuNuoma.UI;

namespace AutomobiliuNuoma
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Server=LAPTOP-2I3V8F0J;Database=CarRentalDB;integrated security = true;";

            var carRepository = new CarRepository(connectionString);
            var customerRepository = new CustomerRepository(connectionString);
            var rentalRepository = new RentalRepository(connectionString);

            var rentService = new RentService(carRepository, customerRepository, rentalRepository);
            var rentConsoleUI = new RentConsoleUI(rentService);

            await rentConsoleUI.StartAsync();
        }
    }
}
