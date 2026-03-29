using TicketBooking.Api.Models.Dtos;

namespace TicketBooking.Api.Services
{
    public interface IShowService
    {
        Task<List<ShowResponseDto>> GetShowsByDayAsync(DateTime date);
        Task<ShowDetailsResponseDto?> GetShowDetailsAsync(int showId);
    }
}
