using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Data;
using TicketBooking.Api.Models.Dtos;

namespace TicketBooking.Api.Services
{
    public class ShowService : IShowService
    {
        private readonly TicketBookingDbContext _context;

        public ShowService(TicketBookingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShowResponseDto>> GetShowsByDayAsync(DateTime date)
        {
            var targetDate = date.Date;
            return await _context.Shows
                .Where(s => s.StartTime.Date == targetDate)
                .Select(s => new ShowResponseDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToListAsync();
        }

        public async Task<ShowDetailsResponseDto?> GetShowDetailsAsync(int showId)
        {
            var show = await _context.Shows.FirstOrDefaultAsync(s => s.Id == showId);
            if (show == null) return null;

            var seats = await _context.Seats
                .Where(s => s.ShowId == showId)
                .ToListAsync();

            // Fetch current bookings for this show to determine availability
            var bookings = await _context.Bookings
                .Where(b => b.ShowId == showId && b.Status != "Cancelled")
                .ToListAsync();

            var response = new ShowDetailsResponseDto
            {
                ShowId = show.Id,
                Title = show.Title,
                Seats = seats.Select(s => new SeatStatusDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Tier = s.Tier,
                    Price = s.BasePrice,
                    Status = bookings.FirstOrDefault(b => b.SeatId == s.Id)?.Status ?? "Available"
                }).ToList()
            };

            return response;
        }
    }
}
