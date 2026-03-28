using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Api.Models
{
    public class Show
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        // Relationship
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
