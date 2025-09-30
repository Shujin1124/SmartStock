using Microsoft.EntityFrameworkCore;
using SmartStock.Domain;

namespace SmartStock.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<ProfileInfo> Profiles { get; set; } // 👈 Add DbSet for Profile table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔗 One Campus has many Accounts
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Campus)
                .WithMany(c => c.Users)
                .HasForeignKey(a => a.CampusId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔗 One-to-One: Account ↔ Profile
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profile)
                .WithOne(p => p.Account)
                .HasForeignKey<ProfileInfo>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
