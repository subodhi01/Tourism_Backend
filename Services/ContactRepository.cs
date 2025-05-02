using Microsoft.Data.SqlClient;
using System.Data;
using TourismGalle.Models;
using Dapper;

namespace TourismGalle.Services
{
    public class ContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Contact>("GetAllContacts", commandType: CommandType.StoredProcedure);
        }

        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Contact>(
                "GetContactByID", new { ContactID = contactId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddContactAsync(Contact contact)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                contact.Name,
                contact.Email,
                contact.Subject,
                contact.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            return await connection.ExecuteAsync("AddContact", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateContactStatusAsync(int contactId, bool isRead)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("UpdateContactStatus",
                new { ContactID = contactId, IsRead = isRead },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteContactAsync(int contactId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("DeleteContact",
                new { ContactID = contactId },
                commandType: CommandType.StoredProcedure);
        }
    }
}