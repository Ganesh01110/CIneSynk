using Microsoft.EntityFrameworkCore;
using TicketBooking.Api.Models;

namespace TicketBooking.Api.Data
{
    public class TicketBookingDbContext : DbContext
    {
        public TicketBookingDbContext(DbContextOptions<TicketBookingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // For Optimistic Concurrency on Booking
            modelBuilder.Entity<Booking>()
                .Property(b => b.RowVersion)
                .IsRowVersion()
                .HasColumnType("TIMESTAMP") // Specifically required for EF + MariaDB/MySQL for IsRowVersion
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
