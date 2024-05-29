// RentConsoleUI.cs
using AutomobiliuNuoma.Services;
using System;
using System.Threading.Tasks;
using AutomobiliuNuoma;
using AutomobiliuNuoma.Services;

namespace AutomobiliuNuoma.UI
{
    public class RentConsoleUI
    {
        private readonly IRentService _rentService;

        public RentConsoleUI(IRentService rentService)
        {
            _rentService = rentService;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                Console.WriteLine("Car Rental Management System");
                Console.WriteLine("1. List all cars");
                Console.WriteLine("2. Add a new car");
                Console.WriteLine("3. Update car information");
                Console.WriteLine("4. Delete a car");
                Console.WriteLine("5. List all customers");
                Console.WriteLine("6. Add a new customer");
                Console.WriteLine("7. Update customer information");
                Console.WriteLine("8. Delete a customer");
                Console.WriteLine("9. Rent a car");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ListAllCarsAsync();
                        break;
                    case "2":
                        await AddCarAsync();
                        break;
                    case "3":
                        await UpdateCarAsync();
                        break;
                    case "4":
                        await DeleteCarAsync();
                        break;
                    case "5":
                        await ListAllCustomersAsync();
                        break;
                    case "6":
                        await AddCustomerAsync();
                        break;
                    case "7":
                        await UpdateCustomerAsync();
                        break;
                    case "8":
                        await DeleteCustomerAsync();
                        break;
                    case "9":
                        await RentCarAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private async Task ListAllCarsAsync()
        {
            var cars = await _rentService.GetAllCarsAsync();
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Id} - {car.Make} {car.Model} ({car.Year}) - {car.RegistrationNumber}");
            }
        }

        private async Task AddCarAsync()
        {
            Console.WriteLine("Enter car make:");
            var make = Console.ReadLine();

            Console.WriteLine("Enter car model:");
            var model = Console.ReadLine();

            Console.WriteLine("Enter car year:");
            if (!int.TryParse(Console.ReadLine(), out var year))
            {
                Console.WriteLine("Invalid year. Operation cancelled.");
                return;
            }

            Console.WriteLine("Enter car registration number:");
            var registrationNumber = Console.ReadLine();

            Console.WriteLine("Enter car type (1 for Oil Fuel, 2 for Electric):");
            if (!int.TryParse(Console.ReadLine(), out var carType) || (carType != 1 && carType != 2))
            {
                Console.WriteLine("Invalid car type. Operation cancelled.");
                return;
            }

            if (carType == 1)
            {
                Console.WriteLine("Enter tank capacity:");
                if (!double.TryParse(Console.ReadLine(), out var tankCapacity))
                {
                    Console.WriteLine("Invalid tank capacity. Operation cancelled.");
                    return;
                }

                var oilFuelCar = new OilFuelCar { Make = make, Model = model, Year = year, RegistrationNumber = registrationNumber, TankCapacity = tankCapacity };
                await _rentService.AddCarAsync(oilFuelCar);
            }
            else
            {
                Console.WriteLine("Enter battery capacity:");
                if (!double.TryParse(Console.ReadLine(), out var batteryCapacity))
                {
                    Console.WriteLine("Invalid battery capacity. Operation cancelled.");
                    return;
                }

                var electricCar = new ElectricCar { Make = make, Model = model, Year = year, RegistrationNumber = registrationNumber, BatteryCapacity = batteryCapacity };
                await _rentService.AddCarAsync(electricCar);
            }

            Console.WriteLine("Car added successfully.");
        }

        private async Task UpdateCarAsync()
        {
            Console.WriteLine("Enter car ID to update:");
            if (!int.TryParse(Console.ReadLine(), out var carId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            var car = await _rentService.GetCarByIdAsync(carId);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            Console.WriteLine($"Current make: {car.Make}");
            Console.WriteLine("Enter new make (or press Enter to keep current):");
            var make = Console.ReadLine();
            if (!string.IsNullOrEmpty(make))
                car.Make = make;

            Console.WriteLine($"Current model: {car.Model}");
            Console.WriteLine("Enter new model (or press Enter to keep current):");
            var model = Console.ReadLine();
            if (!string.IsNullOrEmpty(model))
                car.Model = model;

            Console.WriteLine($"Current year: {car.Year}");
            Console.WriteLine("Enter new year (or press Enter to keep current):");
            if (int.TryParse(Console.ReadLine(), out var year))
                car.Year = year;

            Console.WriteLine($"Current registration number: {car.RegistrationNumber}");
            Console.WriteLine("Enter new registration number (or press Enter to keep current):");
            var registrationNumber = Console.ReadLine();
            if (!string.IsNullOrEmpty(registrationNumber))
                car.RegistrationNumber = registrationNumber;

            if (car is OilFuelCar oilFuelCar)
            {
                Console.WriteLine($"Current tank capacity: {oilFuelCar.TankCapacity}");
                Console.WriteLine("Enter new tank capacity (or press Enter to keep current):");
                if (double.TryParse(Console.ReadLine(), out var tankCapacity))
                    oilFuelCar.TankCapacity = tankCapacity;
            }
            else if (car is ElectricCar electricCar)
            {
                Console.WriteLine($"Current battery capacity: {electricCar.BatteryCapacity}");
                Console.WriteLine("Enter new battery capacity (or press Enter to keep current):");
                if (double.TryParse(Console.ReadLine(), out var batteryCapacity))
                    electricCar.BatteryCapacity = batteryCapacity;
            }

            await _rentService.UpdateCarAsync(car);
            Console.WriteLine("Car updated successfully.");
        }

        private async Task DeleteCarAsync()
        {
            Console.WriteLine("Enter car ID to delete:");
            if (!int.TryParse(Console.ReadLine(), out var carId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            await _rentService.DeleteCarAsync(carId);
            Console.WriteLine("Car deleted successfully.");
        }

        private async Task ListAllCustomersAsync()
        {
            var customers = await _rentService.GetAllCustomersAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id} - {customer.Name} ({customer.Email}, {customer.PhoneNumber})");
            }
        }

        private async Task AddCustomerAsync()
        {
            Console.WriteLine("Enter customer name:");
            var name = Console.ReadLine();

            Console.WriteLine("Enter customer email:");
            var email = Console.ReadLine();

            Console.WriteLine("Enter customer phone number:");
            var phoneNumber = Console.ReadLine();

            var customer = new Customer { Name = name, Email = email, PhoneNumber = phoneNumber };
            await _rentService.AddCustomerAsync(customer);

            Console.WriteLine("Customer added successfully.");
        }

        private async Task UpdateCustomerAsync()
        {
            Console.WriteLine("Enter customer ID to update:");
            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            var customer = await _rentService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.WriteLine($"Current name: {customer.Name}");
            Console.WriteLine("Enter new name (or press Enter to keep current):");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
                customer.Name = name;

            Console.WriteLine($"Current email: {customer.Email}");
            Console.WriteLine("Enter new email (or press Enter to keep current):");
            var email = Console.ReadLine();
            if (!string.IsNullOrEmpty(email))
                customer.Email = email;

            Console.WriteLine($"Current phone number: {customer.PhoneNumber}");
            Console.WriteLine("Enter new phone number (or press Enter to keep current):");
            var phoneNumber = Console.ReadLine();
            if (!string.IsNullOrEmpty(phoneNumber))
                customer.PhoneNumber = phoneNumber;

            await _rentService.UpdateCustomerAsync(customer);
            Console.WriteLine("Customer updated successfully.");
        }

        private async Task DeleteCustomerAsync()
        {
            Console.WriteLine("Enter customer ID to delete:");
            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            await _rentService.DeleteCustomerAsync(customerId);
            Console.WriteLine("Customer deleted successfully.");
        }

        private async Task RentCarAsync()
        {
            Console.WriteLine("Enter car ID to rent:");
            if (!int.TryParse(Console.ReadLine(), out var carId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            Console.WriteLine("Enter customer ID:");
            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            Console.WriteLine("Enter rental start date (YYYY-MM-DD):");
            if (!DateTime.TryParse(Console.ReadLine(), out var fromDate))
            {
                Console.WriteLine("Invalid date. Operation cancelled.");
                return;
            }

            Console.WriteLine("Enter rental end date (YYYY-MM-DD):");
            if (!DateTime.TryParse(Console.ReadLine(), out var toDate))
            {
                Console.WriteLine("Invalid date. Operation cancelled.");
                return;
            }

            var rental = new Rental { CarId = carId, CustomerId = customerId, FromDate = fromDate, ToDate = toDate };

            try
            {
                await _rentService.AddRentalAsync(rental);
                Console.WriteLine("Car rented successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
