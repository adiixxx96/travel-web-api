using Microsoft.EntityFrameworkCore;
using TravelWebApi.Models;

namespace TravelWebApi.Data
{

    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }


     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User{Name = "Admin", Username = "admin", Password = "admin", Role = true, CreationDate = DateTime.Now, UserId=1},
                new User{Name = "Ada", Username = "ada", Password = "ada", Role = false, CreationDate = DateTime.Now, UserId=2}
            );
        }
    }
}
