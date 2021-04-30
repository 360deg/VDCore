using System;
using Microsoft.EntityFrameworkCore;
using VDCore.Authorization;
using VDCore.DBContext.Core.Models;

namespace VDCore.DBContext.Core
{
    public class CoreDbContext: DbContext
    {
        /*
         * VDCore sequences.
         */
        public DbSet<UserStatus> UserStatus { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting default data into variable
            const int adminRoleId = 1;
            const int activeStatusId = 1;
            const long adminUserId = 1;
            const int userRoleId = 1;
            const string adminLogin = "SuperAdmin";
            const string adminPassword = "secret.password";
            
            /*
             * Sets default user roles in VDCore.Roles table
             */
            Role adminRole = new Role() {RoleId = adminRoleId, Name = "Administrator"};
            Role userRole = new Role() {RoleId = adminRoleId + 1, Name = "User"};
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);
            
            /*
             * Sets default user roles in VDCore.UserStatus table
             */
            UserStatus activeStatus = new UserStatus() {UserStatusId = activeStatusId, StatusName = "Active"};
            UserStatus disabledStatus = new UserStatus() {UserStatusId = activeStatusId + 1, StatusName = "Disabled"};
            modelBuilder.Entity<UserStatus>().HasData(activeStatus, disabledStatus);
            
            /*
             * Adds default admin user
             */
            User adminUser = new User()
            {
                UserId = adminUserId,
                Login = adminLogin,
                Password = HashPasswordGenerator.GenerateHash(adminPassword),
                CoreId = Guid.NewGuid(),
                UserStatusId = activeStatus.UserStatusId
            };
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<UserRole>().HasData(new UserRole() {Id = userRoleId, RoleId = adminRole.RoleId, UserId = adminUser.UserId});
        }
    }
}