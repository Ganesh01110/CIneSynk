using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Api.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SeatNumber { get; set; } = string.Empty; // e.g., "A1", "A2"

        [Required]
        public string Tier { get; set; } = "Commoner"; // "Premium", "Balcony", "AC"

        public decimal BasePrice { get; set; }

        // Navigation property for Show
        public int ShowId { get; set; }
        
        [ForeignKey("ShowId")]
        public Show? Show { get; set; }
    }
}
