using Microsoft.EntityFrameworkCore;
using Shazam.Domain.Entity;

namespace Shazam.Persistence.Context
{
    public class ShazamContext : DbContext
    {
        public ShazamContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Song> songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShazamContext).Assembly);
        }
    }
}
