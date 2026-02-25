using NoteApp.API.Models;

namespace NoteApp.API.Repositories;

// Interface for user repository
// This interface defines the contract for user-related data operations
// It includes methods for retrieving users by email, username, and ID, as well as a method for creating a new user
//it talk to the database and perform operations related to user data
public interface IUserRepository
{
    // Method to get a user by email
    Task<User?> GetByEmailAsync(string email);

    // Method to get a user by username
    Task<User?> GetByUsernameAsync(string username);

    // Method to get a user by ID
    Task<User?> GetByIdAsync(Guid id);

    // method to create a new user
    Task<Guid> CreateAsync(User user);
}
    
