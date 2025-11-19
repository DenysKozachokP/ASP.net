using Microsoft.EntityFrameworkCore;

namespace CharityHub.Models
{
    public class CharityDbContext : DbContext
    {
        public CharityDbContext(DbContextOptions<CharityDbContext> options) : base(options) { }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Donation> Donations => Set<Donation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв'язку один-до-багатьох між Event та Donation
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Event)
                .WithMany(e => e.Donations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade); // При видаленні події видаляються всі пожертви
        }
    }
}
