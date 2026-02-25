using Dapper;
using NoteApp.API.Data;
using NoteApp.API.Models;

namespace NoteApp.API.Repositories.Impl;

// Implementation of the INoteRepository interface
// This class provides concrete implementations of the methods defined in the INoteRepository interface
// It uses Dapper to interact with the database and perform operations related to note data
public class NoteRepository : INoteRepository
{
    // Constructor that takes an IDbConnectionFactory as a dependency
    // This allows for dependency injection and makes the class more testable
    // The IDbConnectionFactory is used to create database connections for executing queries
    private readonly IDbConnectionFactory _connectionFactory;

    // Constructor that initializes the NoteRepository with a database connection factory
    public NoteRepository(IDbConnectionFactory connectionFactory)
    {
        // Assign the provided connection factory to the private field
        _connectionFactory = connectionFactory;
    }

    // Method to get all notes by user ID, with optional search and sorting parameters
    public async Task<IEnumerable<Note>> GetAllByUserAsync(Guid userId, string? search, string sortBy)
    {
        // Determine the order clause based on the sortBy parameter
        var orderClause = sortBy switch
        {
            "created_asc" => "CreatedAt ASC",
            "title_asc" => "Title ASC",
            "title_desc" => "Title DESC",
            _ => "CreatedAt DESC"
        };

        // Define the SQL query to retrieve notes based on user ID, search term, and sorting
        var sql = $@"
            SELECT * FROM Notes 
            WHERE UserId = @UserId
                AND (@Search IS NULL OR Title LIKE @SearchPattern OR Content LIKE @SearchPattern)
            ORDER BY {orderClause}";

        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute the query and return the results
        return await conn.QueryAsync<Note>(sql, new
        {
            UserId = userId,
            Search = search,
            // If search is null or whitespace, set SearchPattern to null; otherwise, wrap the search term with wildcards for the LIKE operator
            SearchPattern = string.IsNullOrWhiteSpace(search) ? null : $"%{search}%"
        });
    }

    // Method to get a note by its ID and user ID
    public async Task<Note?> GetByIdAsync(Guid id, Guid userId)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to retrieve a note by its ID and user ID and return the result
        return await conn.QuerySingleOrDefaultAsync<Note>(
            "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId",
            new { Id = id, UserId = userId });
    }

    // Method to create a new note
    public async Task<Guid> CreateAsync(Note note)
    {
        // Generate a new GUID for the note ID
        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to insert a new note into the database and return the generated ID
        await conn.ExecuteAsync(
            @"INSERT INTO Notes (Id, UserId, Title, Content, CreatedAt, UpdatedAt)
              VALUES (@Id, @UserId, @Title, @Content, @CreatedAt, @UpdatedAt)",
            new { Id = id, note.UserId, note.Title, note.Content, CreatedAt = now, UpdatedAt = now });
        
        return id;
    }

    // Method to update an existing note
    public async Task<bool> UpdateAsync(Note note)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to update the note in the database and return whether any rows were affected
        var rows = await conn.ExecuteAsync(
            @"UPDATE Notes 
              SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt 
              WHERE Id = @Id AND UserId = @UserId",
            new { note.Id, note.UserId, note.Title, note.Content, UpdatedAt = DateTime.UtcNow });

        return rows > 0;
    }

    // Method to delete a note by its ID and user ID
    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        // Create a database connection using the connection factory
        using var conn = _connectionFactory.CreateConnection();
        // Execute a query to delete the note from the database and return whether any rows were affected
        var rows = await conn.ExecuteAsync(
            "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId",
            new { Id = id, UserId = userId });

        return rows > 0;
    }
}