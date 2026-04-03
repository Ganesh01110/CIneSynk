using System;
using System.Threading.Tasks;
using Moq;
using TicketBooking.Api.Data;
using TicketBooking.Api.Models;
using TicketBooking.Api.Models.Dtos;
using TicketBooking.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace TicketBooking.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;

    public AuthServiceTests()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["JwtSettings:Key"]).Returns("super_secret_key_that_is_long_enough_for_sha256");
    }

    private TicketBookingDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<TicketBookingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TicketBookingDbContext(options);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenNewUser()
    {
        // Arrange
        using var context = GetDbContext();
        var service = new AuthService(context, _mockConfig.Object);
        var request = new RegisterRequest 
        { 
            Name = "Ganesh", 
            PhoneNumber = "1234567890", 
            Age = 25, 
            Gender = "Male", 
            Password = "SecurePassword123" 
        };

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("User registered successfully.", result.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        using var context = GetDbContext();
        var service = new AuthService(context, _mockConfig.Object);
        
        // Setup existing user
        var password = "RealPassword123";
        var user = new User 
        { 
            Name = "Ganesh", 
            PhoneNumber = "1234567890", 
            Age = 25, 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "User" 
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var loginRequest = new LoginRequest { PhoneNumber = "1234567890", Password = password };

        // Act
        var result = await service.LoginAsync(loginRequest);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.Equal("Ganesh", result.Name);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFail_WhenPasswordIsWrong()
    {
        // Arrange
        using var context = GetDbContext();
        var service = new AuthService(context, _mockConfig.Object);
        
        var user = new User 
        { 
            Name = "Ganesh", 
            PhoneNumber = "1234567890", 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("RealPassword123"),
            Role = "User" 
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var loginRequest = new LoginRequest { PhoneNumber = "1234567890", Password = "WrongPassword" };

        // Act
        var result = await service.LoginAsync(loginRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid phone number or password.", result.Message);
    }
}
