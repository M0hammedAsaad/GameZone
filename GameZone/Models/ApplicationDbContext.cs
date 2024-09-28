using Microsoft.EntityFrameworkCore;

namespace GameZone.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GameDevice>()
                .HasKey(e => new { e.GameId, e.DeviceId });

            modelBuilder.Entity<GameDevice>()
           .HasOne(gd => gd.Game)
           .WithMany(g => g.GameDevices)
           .HasForeignKey(gd => gd.GameId);

            modelBuilder.Entity<GameDevice>()
                .HasOne(gd => gd.Device)
                .WithMany(d => d.GameDevices)
                .HasForeignKey(gd => gd.DeviceId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Games)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CaregoryId)
                .IsRequired(false);
        }


    }


}
