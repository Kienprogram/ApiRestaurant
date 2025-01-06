using apiRestaurant.Model;
using Microsoft.EntityFrameworkCore;

namespace apiRestaurant.Repository
{
    public class PosContext : DbContext
    {
        public PosContext(DbContextOptions<PosContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }  
    }
}
