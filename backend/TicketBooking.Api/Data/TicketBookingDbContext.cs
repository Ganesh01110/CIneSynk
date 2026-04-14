using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Models;

namespace TicketBooking.Api.Data
{
    public class TicketBookingDbContext : DbContext
    {
        public TicketBookingDbContext(DbContextOptions<TicketBookingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Show> Shows { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Booking>()
                .Property(b => b.RowVersion)
                .IsRowVersion();
        }
    }
}
