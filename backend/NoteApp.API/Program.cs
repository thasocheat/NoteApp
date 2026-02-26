using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NoteApp.API.Data;
using NoteApp.API.Helpers;
using NoteApp.API.Repositories;
using NoteApp.API.Repositories.Impl;
using NoteApp.API.Services;
using NoteApp.API.Services.Impl;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NoteApp API", Version = "v1" });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(builder.Configuration["AllowedOrigins"] ?? "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Database
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotesService, NotesService>();

// Helpers
builder.Services.AddSingleton<JwtHelper>();

//JWT Authentication (reads token from HttpOnly cookie)
var jwtHelper = new JwtHelper(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Configure the JWT Bearer options to read the token from the HttpOnly cookie
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // allow to read token from cookie
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtHelper.Issuer,
            ValidAudience = jwtHelper.Audience,
            IssuerSigningKey = jwtHelper.GetSigningKey()
        };

        // Read JWT from HttpOnly cookie instead of Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Check if the request has the "token" cookie
                if (context.Request.Cookies.TryGetValue("auth_token", out var token))
                    context.Token = token; // Set the token for validation
                return Task.CompletedTask;
            }
        };
        
    });

// Authorization
builder.Services.AddAuthorization();


// App Pipeline
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
