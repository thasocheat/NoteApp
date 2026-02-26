using Dapper;
using NoteApp.API.Data;
using NoteApp.API.Models;

namespace NoteApp.API.Repositories.Impl;

// Implementation of the IUserRepository interface
// This class provides concrete implementations of the methods defined in the IUserRepository interface
// It uses Dapper to interact with the database and perform operations related to user data
public class UserRepository : IUserRepository
{
    // Constructor that takes an IDbConnectionFactory as a dependency
    // This allows for dependency injection and makes the class more testable
    // The IDbConnectionFactory is used to create database connections for executing queries
    private readonly IDbConnectionFactory _connectionFactory;


    // Constructor that initializes the UserRepository with a database connection factory
    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        // Assign the provided connection factory to the private field
        _connectionFactory = connectionFactory;
    }

    // Method to get a user by email
    public async Task<User?> GetByEmailAsync(string email)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to retrieve a user by email and return the result
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email = @Email",
            new { Email = email });
    }

    // Method to get a user by username
    public async Task<User?> GetByUsernameAsync(string username)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to retrieve a user by username and return the result
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    // Method to get a user by ID
    public async Task<User?> GetByIdAsync(Guid id)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to retrieve a user by ID and return the result
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id",
            new { Id = id });
    }

    // Method to create a new user
    public async Task<Guid> CreateAsync(User user)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();

        // Generate a new GUID for the user ID
        var id = Guid.NewGuid();

        // Execute a query to insert a new user into the database and return the generated ID
        await conn.ExecuteAsync(
            @"INSERT INTO Users (Id, Username, Email, Password, CreatedAt)
              VALUES (@Id, @Username, @Email, @Password, @CreatedAt)",
            new { Id = id, user.Username, user.Email, user.Password, CreatedAt = DateTime.UtcNow });
        return id;
    }
}
