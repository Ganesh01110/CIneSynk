using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TicketBooking.Api.Data;

namespace TicketBooking.Api.Data
{
    public class TicketBookingDbContextFactory : IDesignTimeDbContextFactory<TicketBookingDbContext>
    {
        public TicketBookingDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TicketBookingDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(10, 4, 32));

            // We use the same configuration as Program.cs
            builder.UseMySql(connectionString!, serverVersion);

            return new TicketBookingDbContext(builder.Options);
        }
    }
}
