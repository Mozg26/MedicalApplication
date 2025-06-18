using IdentityDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityDatabase
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
