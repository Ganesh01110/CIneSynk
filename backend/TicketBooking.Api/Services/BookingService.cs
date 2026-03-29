using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Data;
using TicketBooking.Api.Models;
using TicketBooking.Api.Models.Dtos;

namespace TicketBooking.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly TicketBookingDbContext _context;

        public BookingService(TicketBookingDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse> ReserveSeatAsync(int showId, int seatId, int userId)
        {
            // 1. Check if the seat is already booked or reserved
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.ShowId == showId && b.SeatId == seatId && b.Status != "Cancelled");

            if (existingBooking != null)
            {
                return new AuthResponse { Success = false, Message = "Seat is already taken or reserved." };
            }

            // 2. Create a new "Pending" booking
            var booking = new Booking
            {
                ShowId = showId,
                SeatId = seatId,
                UserId = userId,
                Status = "Pending",
                BookingTime = DateTime.UtcNow
            };

            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return new AuthResponse { Success = true, Message = "Seat reserved. Please pay within 10 minutes.", Token = booking.Id.ToString() };
            }
            catch (DbUpdateException)
            {
                // This catches unique constraint violations or other DB issues
                return new AuthResponse { Success = false, Message = "Concurrency error: Someone else might have just grabbed this seat." };
            }
        }

        public async Task<AuthResponse> ConfirmBookingAsync(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null) return new AuthResponse { Success = false, Message = "Booking not found." };
            if (booking.Status == "Confirmed") return new AuthResponse { Success = false, Message = "Already confirmed." };

            // Simulation: Payment check
            // In a real app, we'd verify with a Payment Gateway here.
            
            booking.Status = "Confirmed";

            try
            {
                await _context.SaveChangesAsync();
                return new AuthResponse { Success = true, Message = "Booking confirmed! Enjoy your show." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new AuthResponse { Success = false, Message = "Concurrency error: The booking state has changed." };
            }
        }

        public async Task<AuthResponse> CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);
            if (booking == null) return new AuthResponse { Success = false, Message = "Booking not found." };

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return new AuthResponse { Success = true, Message = "Booking cancelled." };
        }
    }
}
