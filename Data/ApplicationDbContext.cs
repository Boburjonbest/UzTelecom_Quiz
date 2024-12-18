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
        public DbSet<Register> Registers { get; set;}
        public DbSet<Login> Logins { get; set;}
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.username)
                .IsUnique();

            modelBuilder.Entity<Register>().HasIndex(l => l.Username).IsUnique();
            modelBuilder.Entity<Login>().HasIndex(a => a.Username).IsUnique();
        }
    }
}
