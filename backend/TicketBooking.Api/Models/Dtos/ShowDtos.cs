namespace TicketBooking.Api.Models.Dtos
{
    public class ShowResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class SeatStatusDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string Tier { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = "Available"; // "Available", "Booked", "Locked"
    }

    public class ShowDetailsResponseDto
    {
        public int ShowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<SeatStatusDto> Seats { get; set; } = new();
    }
}
