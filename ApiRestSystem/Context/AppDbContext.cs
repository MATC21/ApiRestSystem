using ApiRestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRestSystem.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { 

        }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
