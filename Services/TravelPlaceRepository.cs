using Microsoft.Data.SqlClient;
using System.Data;
using TourismGalle.Models;
using Dapper;

namespace TourismGalle.Services
{
    public class TravelPlaceRepository
    {
        private readonly string _connectionString;

        public TravelPlaceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<TravelPlace>> GetAllTravelPlacesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TravelPlace>("GetAllTravelPlaces", commandType: CommandType.StoredProcedure);
        }

        public async Task<TravelPlace> GetTravelPlaceByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var place = await connection.QueryFirstOrDefaultAsync<TravelPlace>(
                "GetTravelPlaceById", new { Id = id }, commandType: CommandType.StoredProcedure);

            if (place != null)
            {
                // Get facilities for this place
                place.Facilities = (await connection.QueryAsync<TravelPlaceFacility>(
                    "GetTravelPlaceFacilities", 
                    new { TravelPlaceId = id }, 
                    commandType: CommandType.StoredProcedure)).ToList();
            }

            return place;
        }

        public async Task<int> AddTravelPlaceAsync(TravelPlace place, List<TravelPlaceFacility> facilities)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var parameters = new
                {
                    place.OwnerName,
                    place.OwnerEmail,
                    place.PlaceName,
                    place.Description,
                    place.LocationLink,
                    place.Images,
                    place.BookingInstructions,
                    place.DiscountNotices,
                    place.ContactInfo,
                    place.BookedDates,
                    place.IsActive
                };

                var placeId = await connection.QuerySingleAsync<int>(
                    "AddTravelPlace", parameters, transaction, commandType: CommandType.StoredProcedure);

                // Add facilities
                foreach (var facility in facilities)
                {
                    facility.TravelPlaceId = placeId;
                    await AddFacilityAsync(facility, connection, transaction);
                }

                transaction.Commit();
                return placeId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task UpdateTravelPlaceAsync(TravelPlace place, List<TravelPlaceFacility> facilities)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Update main travel place
                var parameters = new
                {
                    place.Id,
                    place.OwnerName,
                    place.OwnerEmail,
                    place.PlaceName,
                    place.Description,
                    place.LocationLink,
                    place.Images,
                    place.BookingInstructions,
                    place.DiscountNotices,
                    place.ContactInfo,
                    place.BookedDates,
                    place.IsActive,
                    UpdatedAt = DateTime.UtcNow
                };

                await connection.ExecuteAsync(
                    "UpdateTravelPlace",
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );

                // Delete existing facilities
                await connection.ExecuteAsync(
                    "DeleteTravelPlaceFacilities",
                    new { TravelPlaceId = place.Id },
                    transaction,
                    commandType: CommandType.StoredProcedure
                );

                // Add updated facilities
                foreach (var facility in facilities)
                {
                    var facilityParams = new
                    {
                        facility.Id,
                        TravelPlaceId = place.Id,
                        facility.Name,
                        facility.Description,
                        facility.AveragePrice,
                        facility.PricePerPerson,
                        facility.Duration,
                        facility.Availability,
                        facility.SpecialNotices,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await connection.ExecuteAsync(
                        "AddTravelPlaceFacility",
                        facilityParams,
                        transaction,
                        commandType: CommandType.StoredProcedure
                    );
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task DeleteTravelPlaceAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DeleteTravelPlace", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        private async Task<string> GetFacilitiesForPlaceAsync(int placeId)
        {
            using var connection = new SqlConnection(_connectionString);
            var facilities = await connection.QueryAsync<TravelPlaceFacility>(
                "GetTravelPlaceFacilities", new { TravelPlaceId = placeId }, 
                commandType: CommandType.StoredProcedure);
            return System.Text.Json.JsonSerializer.Serialize(facilities);
        }

        private async Task AddFacilityAsync(TravelPlaceFacility facility, SqlConnection connection, SqlTransaction transaction)
        {
            var parameters = new
            {
                facility.TravelPlaceId,
                facility.Name,
                facility.Description,
                facility.AveragePrice,
                facility.PricePerPerson,
                facility.Duration,
                facility.Availability,
                facility.SpecialNotices
            };

            await connection.ExecuteAsync(
                "AddTravelPlaceFacility", parameters, transaction, commandType: CommandType.StoredProcedure);
        }
    }
} 