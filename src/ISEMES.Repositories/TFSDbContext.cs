using Microsoft.EntityFrameworkCore;

namespace ISEMES.Repositories
{
    public class TFSDbContext : DbContext
    {
        public TFSDbContext(DbContextOptions<TFSDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}



