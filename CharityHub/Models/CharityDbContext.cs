using Microsoft.EntityFrameworkCore;

namespace CharityHub.Models
{
    public class CharityDbContext : DbContext
    {
        public CharityDbContext(DbContextOptions<CharityDbContext> options) : base(options) { }

        public DbSet<Event> Events => Set<Event>();
    }
}
