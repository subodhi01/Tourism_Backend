using Microsoft.Data.SqlClient;
using System.Data;
using TourismGalle.Models;
using Dapper;

namespace TourismGalle.Services
{
    public class PlacesRepository
    {
        private readonly string _connectionString;

        public PlacesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Place>("GetAllPlaces", commandType: CommandType.StoredProcedure);
        }

        public async Task<Place> GetPlaceByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Place>(
                "GetPlaceByID", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Place>> GetPlacesByLocationAsync(string location)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Place>(
                "GetPlacesByLocation", new { Location = location }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddPlaceAsync(Place place)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                place.Name,
                place.Description,
                place.Images,
                place.Price,
                place.Packages,
                place.Capacity,
                place.TimeSlots,
                place.SpecialFunctions,
                place.Location,
                place.ContactInfo,
                place.IsActive
            };
            return await connection.ExecuteAsync("AddPlace", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdatePlaceAsync(Place place)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                place.Id,
                place.Name,
                place.Description,
                place.Images,
                place.Price,
                place.Packages,
                place.Capacity,
                place.TimeSlots,
                place.SpecialFunctions,
                place.Location,
                place.ContactInfo,
                place.IsActive
            };
            return await connection.ExecuteAsync("UpdatePlace", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeletePlaceAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("DeletePlace", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
} 