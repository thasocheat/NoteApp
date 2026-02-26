using NoteApp.API.DTOs;
using NoteApp.API.Models;
using NoteApp.API.Repositories;
using NoteApp.API.Helpers;

namespace NoteApp.API.Services.Impl;

// Implementatiion of the authentication service
// This class provides the logic for user registration and login
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    // Constructor that takes a user repository as a dependency
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Method to register a new user
    public async Task<(bool Success, string? Error, User? User)> RegisterAsync(RegisterRequest request)
    {
        // Check uniqueness of the email and username
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            return (false, "Email is already in use.", null);

        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
            return (false, "Username is already in use.", null);

        // Create a new user object
        var user = new User
        {
            Username = request.Username.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            Password = PasswordHelper.Hash(request.Password)
        };

        // Save the user to the repository
        user.Id = await _userRepository.CreateAsync(user);
        return (true, null, user);
    }
        

    public async Task<(bool Success, string? Error, User? User)> LoginAsync(LoginRequest request)
    {
        // Find the user by email
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());

        // If user not found or password does not match, return an error
        if (user is null || !PasswordHelper.Verify(request.Password, user.Password))
            return (false, "Invalid email or password.", null);

        // If login is successful, return the user
        return (true, null, user);
        
    }
}