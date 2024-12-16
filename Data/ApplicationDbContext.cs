using Microsoft.EntityFrameworkCore;
using UzTelecom_Quiz.Models;

namespace UzTelecom_Quiz.Data
{
    public class ApplicationDbContext : DbContext
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {

        }

        public DbSet<User> Users { get; set;}
        public DbSet<Password> Passwords { get; set;}
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.username)
                .IsUnique();
        }
    }
}
