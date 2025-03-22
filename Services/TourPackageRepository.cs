using Microsoft.Data.SqlClient;
using System.Data;
using TourismGalle.Models;
using Dapper;



namespace TourismGalle.Services
{
    public class TourPackageRepository
    {
        private readonly string _connectionString;

        public TourPackageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<TourPackage>> GetAllTourPackagesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TourPackage>("GetAllTourPackages", commandType: CommandType.StoredProcedure);
        }

        public async Task<TourPackage> GetTourPackageByIdAsync(int packageId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<TourPackage>(
                "GetTourPackageByID", new { PackageID = packageId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TourPackage>> GetTourPackagesByPlaceAsync(string place)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TourPackage>(
                "GetTourPackagesByPlace", new { Place = place }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddTourPackageAsync(TourPackage package)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                package.PackageName,
                package.Description,
                package.Price,
                package.DurationDays,
                package.Place
            };
            return await connection.ExecuteAsync("AddTourPackage", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateTourPackageAsync(TourPackage package)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                package.PackageID,
                package.PackageName,
                package.Description,
                package.Price,
                package.DurationDays,
                package.Place
            };
            return await connection.ExecuteAsync("UpdateTourPackage", parameters, commandType: CommandType.StoredProcedure);
        }


        public async Task<int> DeleteTourPackageAsync(int packageId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("DeleteTourPackage", new { PackageID = packageId }, commandType: CommandType.StoredProcedure);
        }
    }
}
