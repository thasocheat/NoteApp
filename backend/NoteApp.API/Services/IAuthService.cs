using NoteApp.API.DTOs;
using NoteApp.API.Models;

namespace NoteApp.API.Services;

// Interface for authentication service
// This interface defines the contract for authentication-related operations
// It includes methods for user registration, login, and token generation
public interface IAuthService
{
    // Method to register a new user
    Task<(bool Success, string? Error, User? User)> RegisterAsync(RegisterRequest request);

    // Method to log in a user
    Task<(bool Success, string? Error, User? User)> LoginAsync(LoginRequest request);
}