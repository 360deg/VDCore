using Microsoft.EntityFrameworkCore;

namespace VDCore.DBContext
{
    public class CoreDbContext: DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Test> Test { get; set; }
    }
}