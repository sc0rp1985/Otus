using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    internal class DbInitializer : IDbInitializer
    {

        public DbInitializer()
        {            
        }
        
        public async Task InitializeAsync(string connectionString)
        {
            var fullConn = connectionString;
            var builder = new NpgsqlConnectionStringBuilder(fullConn);
            var dbName = builder.Database;

            // Подключение к postgres (без указания базы)
            var adminConnStr = new NpgsqlConnectionStringBuilder(fullConn)
            {
                Database = "postgres"
            }.ToString();

            // Проверка существования базы
            using (var conn = new NpgsqlConnection(adminConnStr))
            {
                await conn.OpenAsync();
                var dbExists = await conn.ExecuteScalarAsync<bool>(
                    "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @name)",
                    new { name = dbName });

                if (!dbExists)
                {
                    await conn.ExecuteAsync($"CREATE DATABASE \"{dbName}\"");
                }
            }

            // Подключение к самой базе
            using (var conn = new NpgsqlConnection(fullConn))
            {
                await conn.OpenAsync();

                var createTableSql = @"
                     CREATE TABLE IF NOT EXISTS users (
        id SERIAL PRIMARY KEY,
        first_name TEXT NOT NULL,
        second_name TEXT NOT NULL,
        birthdate DATE NOT NULL,
        biography TEXT NOT NULL,
        city TEXT NOT NULL,
        password TEXT NOT NULL
    );
                ";

                await conn.ExecuteAsync(createTableSql);
            }
        }
    }
}
