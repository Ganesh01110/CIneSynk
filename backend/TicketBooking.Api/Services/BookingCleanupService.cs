using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Data;

namespace TicketBooking.Api.Services
{
    public class BookingCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<BookingCleanupService> _logger;

        public BookingCleanupService(IServiceProvider services, ILogger<BookingCleanupService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Booking Cleanup Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run every 1 minute
            }

            _logger.LogInformation("Booking Cleanup Service is stopping.");
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TicketBookingDbContext>();

                var expiryTime = DateTime.UtcNow.AddMinutes(-10);

                var expiredBookings = await context.Bookings
                    .Where(b => b.Status == "Pending" && b.BookingTime < expiryTime)
                    .ToListAsync(stoppingToken);

                if (expiredBookings.Any())
                {
                    _logger.LogInformation($"Found {expiredBookings.Count} expired bookings. Cleaning up...");
                    foreach (var booking in expiredBookings)
                    {
                        booking.Status = "Cancelled";
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}
