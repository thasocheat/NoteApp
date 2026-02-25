using BCrypt.Net;

namespace NoteApp.API.Helpers;

// Helper class for password hashing and verification
public static class PasswordHelper
{
    // Hash a password using BCrypt
    public static string Hash(string plainPassword)
        => BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);

    // Verify a plain password against a hashed password
    public static bool Verify(string plainPassword, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
}