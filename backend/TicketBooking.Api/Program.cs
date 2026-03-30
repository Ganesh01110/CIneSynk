using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Data;
using TicketBooking.Api.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using TicketBooking.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Configure MariaDB DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"DEBUG: Connection String found: {(!string.IsNullOrEmpty(connectionString))}");
var serverVersion = new MySqlServerVersion(new Version(10, 4, 32)); // Common MariaDB version for XAMPP
builder.Services.AddDbContext<TicketBookingDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

// Configure Health Checks
builder.Services.AddHealthChecks();

// Register Custom Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Register Background Services
builder.Services.AddHostedService<BookingCleanupService>();

// Enable In-Memory Caching (for Idempotency)
builder.Services.AddMemoryCache();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "super_secret_key_that_is_long_enough_for_sha256");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});

// Configure Rate Limiting (Token Bucket)
builder.Services.AddRateLimiter(options => {
    options.AddTokenBucketLimiter("fixed", limiterOptions => {
        limiterOptions.TokenLimit = 100;
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        limiterOptions.TokensPerPeriod = 10;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });
});

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Global Exception Handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// Idempotency Handler
app.UseMiddleware<IdempotencyMiddleware>();

app.UseHttpsRedirection();

// Add Rate Limiting & Auth
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint
app.MapHealthChecks("/health");

app.MapControllers(); // Map Controller routes

app.MapGet("/", () => "Ticket Booking API is running!");

app.Run();
