﻿using Dapper;
using Npgsql;
using System.Data;
using UserManagementSystem.DAL.Models;

namespace UserManagementSystem.DAL.Repositories
{
    public class PhoneRepository
    {
        private readonly string _connectionString;

        public PhoneRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("DbSettings:ConnectionString").Value;
        }

        public async Task CreateUserPhone(UserPhoneDal userPhone)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("INSERT INTO public.users_phones_table (user_id, phone_number, created_at) VALUES(@UserId, @PhoneNumber, @CreatedAt);", userPhone);
            }
        }

        public async Task UpdateUserPhone(UserPhoneDal userPhone)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("UPDATE public.users_phones_table SET  phone_number=@PhoneNumber, updated_at=@UpdatedAt WHERE id=@Id;", userPhone);
            }
        }

        public async Task<List<UserPhoneDal>> GetUserPhonesList(long userId)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                return (await db.QueryAsync<UserPhoneDal>("SELECT id AS Id, user_id AS UserId, phone_number AS PhoneNumber, created_at AS CreatedAt, updated_at AS UpdatedAt FROM public.users_phones_table WHERE user_id=@userId;", new { userId })).ToList();
            }
        }

        public async Task DeletePhoneByUserId(long userId)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("DELETE FROM public.users_phones_table WHERE user_id=@userId;", new { userId });
            }
        }

        public async Task DeletePhone(string phoneNumber)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync("DELETE FROM public.users_phones_table WHERE phone_number=@phoneNumber;", new { phoneNumber });
            }
        }
    }
}
