using System.ComponentModel.DataAnnotations;

namespace NoteApp.API.DTOs;

// DTOs for authentication-related operations
// These classes are used to transfer data between
// the client and the server for authentication purposes
public class RegisterRequest
{
    // properties and validation attributes for user registration
    [Required, MinLength(3), MaxLength(100)]
    public string Username { get; set; } = string.Empty; // Username for registration

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty; // Email for registration

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty; // Password for registration

}

// DTO for user Login
public class LoginRequest
{
    // properties and validation attributes for user login
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty; // Email for login

    [Required]
    public string Password { get; set; } = string.Empty; // Password for login

}

// DTO for authentication response
public class AuthResponse
{
    // properties
    public Guid Id { get; set; } // Unique identifier for the authenticated user
    public string Username { get; set; } = string.Empty; // Username of the authenticated user
    public string Email { get; set; } = string.Empty; // Email of the authenticated user    
}