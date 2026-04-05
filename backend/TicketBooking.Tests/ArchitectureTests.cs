using NetArchTest.Rules;
using TicketBooking.Api.Controllers;
using TicketBooking.Api.Data;
using Xunit;

namespace TicketBooking.Tests.Architecture;

public class ArchitectureTests
{
    private const string ApiNamespace = "TicketBooking.Api";

    [Fact]
    public void Controllers_ShouldNotHaveDependencyOnData()
    {
        // Arrange
        var result = Types.InAssembly(typeof(AuthController).Assembly)
            .That()
            .ResideInNamespace($"{ApiNamespace}.Controllers")
            .ShouldNot()
            .HaveDependencyOn($"{ApiNamespace}.Data")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Controllers should not depend directly on the Data/DbContext layer. Use Services instead.");
    }

    [Fact]
    public void Services_ShouldResideInServicesNamespace()
    {
        // Arrange
        var result = Types.InAssembly(typeof(AuthController).Assembly)
            .That()
            .ImplementInterface(typeof(TicketBooking.Api.Services.IAuthService).GetInterfaces().FirstOrDefault() ?? typeof(TicketBooking.Api.Services.IAuthService))
            .Should()
            .ResideInNamespace($"{ApiNamespace}.Services")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "All service implementations should reside in the .Services namespace.");
    }
}
