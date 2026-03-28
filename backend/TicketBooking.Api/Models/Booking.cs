using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int ShowId { get; set; }
        [ForeignKey("ShowId")]
        public Show? Show { get; set; }

        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public Seat? Seat { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"

        public DateTime BookingTime { get; set; } = DateTime.UtcNow;

        [Timestamp] // Concurrency Token (Optimistic concurrency control)
        public byte[]? RowVersion { get; set; }
    }
}
