using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutomobiliuNuoma;
using System.Data.SqlClient;
using AutomobiliuNuoma.Repositories;

namespace AutomobiliuNuoma.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly string _connectionString;

        public CarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            using (var db = Connection)
            {
                return await db.QueryAsync<Car>("SELECT * FROM Cars");
            }
        }

        public async Task<IEnumerable<OilFuelCar>> GetOilFuelCarsAsync()
        {
            using (var db = Connection)
            {
                var sql = @"SELECT c.*, ofc.TankCapacity FROM Cars c
                            INNER JOIN OilFuelCars ofc ON c.Id = ofc.CarId";
                return await db.QueryAsync<OilFuelCar>(sql);
            }
        }

        public async Task<IEnumerable<ElectricCar>> GetElectricCarsAsync()
        {
            using (var db = Connection)
            {
                var sql = @"SELECT c.*, ec.BatteryCapacity FROM Cars c
                            INNER JOIN ElectricCars ec ON c.Id = ec.CarId";
                return await db.QueryAsync<ElectricCar>(sql);
            }
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            using (var db = Connection)
            {
                return await db.QueryFirstOrDefaultAsync<Car>("SELECT * FROM Cars WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task AddCarAsync(Car car)
        {
            using (var db = Connection)
            {
                var sql = @"INSERT INTO Cars (Make, Model, Year, RegistrationNumber) 
                            VALUES (@Make, @Model, @Year, @RegistrationNumber);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                var carId = await db.QuerySingleAsync<int>(sql, car);

                if (car is OilFuelCar oilFuelCar)
                {
                    var oilFuelSql = @"INSERT INTO OilFuelCars (CarId, TankCapacity) VALUES (@CarId, @TankCapacity)";
                    await db.ExecuteAsync(oilFuelSql, new { CarId = carId, oilFuelCar.TankCapacity });
                }
                else if (car is ElectricCar electricCar)
                {
                    var electricSql = @"INSERT INTO ElectricCars (CarId, BatteryCapacity) VALUES (@CarId, @BatteryCapacity)";
                    await db.ExecuteAsync(electricSql, new { CarId = carId, electricCar.BatteryCapacity });
                }
            }
        }

        public async Task UpdateCarAsync(Car car)
        {
            using (var db = Connection)
            {
                var sql = @"UPDATE Cars SET Make = @Make, Model = @Model, Year = @Year, RegistrationNumber = @RegistrationNumber
                            WHERE Id = @Id";
                await db.ExecuteAsync(sql, car);

                if (car is OilFuelCar oilFuelCar)
                {
                    var oilFuelSql = @"UPDATE OilFuelCars SET TankCapacity = @TankCapacity WHERE CarId = @CarId";
                    await db.ExecuteAsync(oilFuelSql, new { CarId = car.Id, oilFuelCar.TankCapacity });
                }
                else if (car is ElectricCar electricCar)
                {
                    var electricSql = @"UPDATE ElectricCars SET BatteryCapacity = @BatteryCapacity WHERE CarId = @CarId";
                    await db.ExecuteAsync(electricSql, new { CarId = car.Id, electricCar.BatteryCapacity });
                }
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            using (var db = Connection)
            {
                var sql = @"DELETE FROM Cars WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}

// Implement CustomerRepository and RentalRepository similarly
