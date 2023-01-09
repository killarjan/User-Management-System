using Dapper;
using Npgsql;
using System.Data;
using UserManagementSystem.DAL.Models;

namespace UserManagementSystem.DAL.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("DbSettings:ConnectionString").Value;
        }

        public async Task<UserDal[]> GetUsersList()
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                return (await db.QueryAsync<UserDal>("SELECT id, name, age, email, created_at FROM public.users_table;")).ToArray();
            }
        }

        public async Task<UserDal> GetUser(long id)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                return (await db.QueryAsync<UserDal>("SELECT id, name, age, email, created_at FROM public.users_table WHERE Id = @id;", new { id })).FirstOrDefault();
            }
        }

        public async Task<UserFullDataDal[]> AltGetUserFullData(long id)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                return (await db.QueryAsync<UserFullDataDal>("SELECT users_table.id, name, age, email, phone_number AS PhoneNumber FROM users_table LEFT JOIN users_phones_table ON users_phones_table.user_id  = users_table.id WHERE users_table.id = @id;", new { id })).ToArray();
            }
        }

        public async Task CreateUser(UserDal user)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("INSERT INTO public.users_table (name, age, email, created_at) VALUES(@Name, @Age, @Email, @CreatedAt);", user);
            }
        }

        public async Task UpdateUser(UserDal user)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("UPDATE public.users_table SET name = @Name, age = @Age, email = @Email WHERE id=@Id;", user);
            }
        }

        public async Task DeleteUser(long id)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("DELETE FROM public.users_table WHERE id=@id;", new { id });
            }
        }
    }
}
