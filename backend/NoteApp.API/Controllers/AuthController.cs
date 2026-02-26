using NoteApp.API.DTOs;
using NoteApp.API.Helpers;
using NoteApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NoteApp.API.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtHelper _jwtHelper;

    // Constructor to inject the authentication service and JWT helper
    public AuthController(IAuthService authService, JwtHelper jwtHelper)
    {
        _authService = authService;
        _jwtHelper = jwtHelper;
    }

    // POST: api/auth/Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Validate the incoming request model
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Call the authentication service to register the user
        var (success, error, user) = await _authService.RegisterAsync(request);
        if (!success) return Conflict(new { message = error });

        // Generate a JWT token for the newly registered user
        var token = _jwtHelper.GenerateToken(user!);
        AttachTokenCookie(token);

        // Return the user information along with the token
        return Ok(new AuthResponse { Id = user!.Id, Username = user.Username, Email = user.Email });

    }

    // POST: api/auth/Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Validate the incoming request model
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Call the authentication service to authenticate the user
        var (Success, error, User) = await _authService.LoginAsync(request);
        if (!Success) return Unauthorized(new { message = error });

        // Generate a JWT token for the authenticated user
        var token = _jwtHelper.GenerateToken(User!);
        AttachTokenCookie(token);

        // Return the user information along with the token
        return Ok(new AuthResponse { Id = User!.Id, Username = User.Username, Email = User.Email });
    }

    // GET: api/auth/me
    // This endpoint is protected and requires authentication to access the user's information
    // and secure on page refresh to keep the user logged in and on logout
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub"); // Try both standard and custom claim types for user ID
        
        var email = User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue("email"); // Try both standard and custom claim types for email

        var username = User.FindFirstValue("username"); // Custom claim type for username

        // check if the user ID and email are present in the claims, if not return an unauthorized response
        if (id == null || email == null || username == null)
            return Unauthorized(new { message = "Invalid token claims." });
        
        // Return the user information
        return Ok(new AuthResponse { 
            Id = Guid.Parse(id), 
            Email = email, 
            Username = username 
        });
    }

    // POST: api/auth/Logout
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // Clear the authentication cookie to log the user out
        Response.Cookies.Delete("auth_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        // Return a success response indicating the user has been logged out
        return Ok(new { message = "Logged out successfully." });
    }





    // Attach the JWT token as an HTTP-only cookie
    private void AttachTokenCookie(string token)
    {
        Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(24)
        });
    }
}