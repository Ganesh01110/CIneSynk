using TicketBooking.Api.Models.Dtos;

namespace TicketBooking.Api.Services
{
    public interface IBookingService
    {
        Task<AuthResponse> ReserveSeatAsync(int showId, int seatId, int userId);
        Task<AuthResponse> ConfirmBookingAsync(int bookingId, int userId);
        Task<AuthResponse> CancelBookingAsync(int bookingId, int userId);
    }
}
// Using AuthResponse as a generic response container for now to save time, 
// normally we would have a dedicated BaseResponse.
