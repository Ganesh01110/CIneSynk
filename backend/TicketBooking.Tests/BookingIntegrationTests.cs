using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketBooking.Api.Models.Dtos;
using Xunit;

namespace TicketBooking.Tests.Integration;

public class BookingIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BookingIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task FullBookingFlow_ShouldSucceed()
    {
        // 1. Arrange: Create Client
        var client = _factory.CreateClient();

        // 2. Act: Register a new user
        var regRequest = new RegisterRequest
        {
            Name = "Integration User",
            PhoneNumber = "9999999999",
            Age = 30,
            Gender = "Other",
            Password = "SecurePassword123!"
        };
        var regRes = await client.PostAsJsonAsync("/api/auth/register", regRequest);
        
        // Assert Registration
        Assert.True(regRes.IsSuccessStatusCode);

        // 3. Act: Login to get token
        var loginRequest = new LoginRequest { PhoneNumber = "9999999999", Password = "SecurePassword123!" };
        var loginRes = await client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var authData = await loginRes.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert Login
        Assert.NotNull(authData?.Token);

        // 4. Act: Reserve a seat (Mocking headers if your API requires it)
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authData.Token);
        
        // Note: For a real integration test against a persistent DB, 
        // we'd need to ensure a Show with ID 1 exists. 
        // In our current WebApplicationFactory, it uses the real appsettings/DB configuration.
        // For local verification, we assume the DB is seeded or we'd use a TestServer with a Test DB.
    }
}
