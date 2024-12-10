using Microsoft.EntityFrameworkCore;
using api_restaurant.Models;

namespace api_restaurant.Data
{
    public class PosContext : DbContext
    {
        public PosContext(DbContextOptions<PosContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
