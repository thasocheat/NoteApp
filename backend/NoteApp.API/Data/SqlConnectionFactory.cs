using Microsoft.Data.SqlClient;
using System.Data;

namespace NoteApp.API.Data;

// This class is responsible for creating and managing
// SQL database connections. It implements the IConnectionFactory
// interface, which defines a method for creating a new
// database connection.


// The IDbConnectionFactory interface defines a method for creating
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

// The SqlConnectionFactory class implements the IDbConnectionFactory
public class SqlConnectionFactory : IDbConnectionFactory
{
    // The constructor takes a connection string as a parameter and
    // initializes the _connectionString field with it.
    private readonly string _connectionString;

    // The constructor takes an IConfiguration object as a parameter, 
    // which is used to retrieve the connection string from the application's configuration settings.
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    // The CreateConnection method creates and returns a new SqlConnection
    // using the connection string stored in the _connectionString field.
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}