using EMS_MiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_MiniProject.DataContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
