using Microsoft.IdentityModel.Tokens;
using NoteApp.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteApp.API.Helpers;

// Helper class for generating JWT tokens
// This class is responsible for creating JWT tokens that can be used
// for authenticating users in the NoteApp API.
public class JwtHelper
{
    // properties for JWT token generation
    private readonly string _secret; // Secret key used for signing the JWT token
    private readonly string _issuer; // Issuer of the JWT token (usually the application name or URL)
    private readonly string _audience; // Audience for the JWT token (usually the intended recipients of the token)
    private readonly int _expiryHours; // Expiry time for the JWT token in hours

    // Constructor to initialize the JWT helper with configuration values
    public JwtHelper(IConfiguration configuration)
    {
        // Read JWT configuration values from the appsettings.json file

        // The secret key is essential for signing the JWT token, so we throw an exception if it's not configured.
        _secret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("JWT:Secret is not configured.");

        // The issuer and audience have default values of "NoteApp" if not configured, and the expiry time defaults to 24 hours.
        _issuer = configuration["Jwt:Issuer"] ?? "NoteApp";

        // The audience is the intended recipient of the token, and it defaults to "NoteApp" if not configured.
        _audience = configuration["jwt:Audience"] ?? "NoteApp";

        // The expiry time for the token is read from the configuration, and it defaults to 24 hours if not configured. We parse it as an integer.
        _expiryHours = int.Parse(configuration["Jwt:ExpiryHours"] ?? "24");
        
    }

    // Method to generate a JWT token for a given user
    public string GenerateToken(User user)
    {
        // Create a symmetric security key using the secret key
        // The secret key is used to sign the JWT token, 
        // ensuring that it cannot be tampered with. 
        // We convert the secret string into a byte array and create a SymmetricSecurityKey object.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create claims for the JWT token. Claims are pieces of information
        // about the user that are included in the token.
        // In this case, we include the user's ID and username as claims.
        var claims = new[]
        {
            // The "sub" (subject) claim is a standard claim that represents the unique identifier of the user.
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject claim with the user's ID
            new Claim(JwtRegisteredClaimNames.Email, user.Email), // Unique name claim with the user's email
            new Claim("username", user.Username), // Custom claim for the user's username
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID claim with a unique identifier for the token

        };

        // Create the JWT token using the JwtSecurityToken class
        var token = new JwtSecurityToken(
            issuer: _issuer, // The issuer of the token
            audience: _audience, // The intended audience of the token
            claims: claims, // The claims to include in the token
            expires: DateTime.UtcNow.AddHours(_expiryHours), // The expiration time for the token
            signingCredentials: creds // The signing credentials to sign the token
        );

        // Return the serialized JWT token as a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Method to get the signing key for validating JWT tokens
    public SymmetricSecurityKey GetSigningKey()
        => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

    // Properties to expose the issuer and audience for token validation
    public string Issuer => _issuer;
    public string Audience => _audience;
}