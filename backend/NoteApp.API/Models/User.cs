namespace NoteApp.API.Models;

// User model representing a user in the system
public class User
{
    // properties
    public Guid Id { get; set; } // Unique identifier for the user
    public string Username { get; set; } = string.Empty; // Unique username for login
    public string Email { get; set; } = string.Empty; // Unique email address for the user
    public string Password { get; set; } = string.Empty; // BCrypt hash of the password
    public DateTime CreatedAt { get; set; } // Timestamp when the user was created

}