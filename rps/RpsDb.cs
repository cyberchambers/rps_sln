using Microsoft.EntityFrameworkCore;

namespace rps
{
    public class RpsDb : DbContext
    {
        public RpsDb(DbContextOptions<RpsDb> options) : base(options) { }

        public DbSet<RpsMatch> RpsMatches => Set<RpsMatch>();
    }
}
