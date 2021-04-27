using Microsoft.EntityFrameworkCore;
using VDCore.DBContext.Core.Models;

namespace VDCore.DBContext.Core
{
    public class CoreDbContext: DbContext
    {
        /*
         * VDCore sequences.
         */
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * Sets default user roles in VDCore.Roles table
             */
            modelBuilder.Entity<Role>().HasData(
                new Role() {RoleId = 1, Name = "Administrator"},
                new Role() {RoleId = 2, Name = "User"}
            );
        }
    }
}