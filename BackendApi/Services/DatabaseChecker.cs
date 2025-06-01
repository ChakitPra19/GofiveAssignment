using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DatabaseChecker
{
    private readonly string _connectionString;

    public DatabaseChecker(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MyDatabase");
    }

    public bool TestConnection()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT 1", connection);
            var result = command.ExecuteScalar();

            Console.WriteLine("TestConnection result: " + result);
            return result != null && Convert.ToInt32(result) == 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection test failed: " + ex.Message);
            return false;
        }
    }

    public string GetCurrentDatabase()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT DB_NAME()", connection);
            var dbName = command.ExecuteScalar()?.ToString();

            Console.WriteLine("Connected to database: " + dbName);
            return dbName;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to get database name: " + ex.Message);
            return null;
        }
    }
}
