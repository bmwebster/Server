using Microsoft.EntityFrameworkCore;

namespace Server.GraphQL.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=data.db");

        public DbSet<Speaker> Speakers => Set<Speaker>(); // Note we initialise to a set to avoid null
        public DbSet<Topic> Topics => Set<Topic>();
    }
}