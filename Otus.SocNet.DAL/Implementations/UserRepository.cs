using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    internal class UserRepository: IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<User?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "SELECT id, first_name, second_name, birthdate, biography, city, password FROM users WHERE id = @id";
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { id });
        }       

        public async Task<int> CreateAsync(User user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        INSERT INTO users (first_name, second_name, birthdate, biography, city, password)
        VALUES (@First_Name, @Second_Name, @Birthdate, @Biography, @City, @Password)
        RETURNING id;
    ";
            var id = await conn.ExecuteScalarAsync<int>(sql, user);
            return id;
        }
    }
}

