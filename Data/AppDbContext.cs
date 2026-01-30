using Microsoft.EntityFrameworkCore;
using GestionDeTurnos.Web.Models;

namespace GestionDeTurnos.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Declaración de las tablas de la base de datos
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
    }
}