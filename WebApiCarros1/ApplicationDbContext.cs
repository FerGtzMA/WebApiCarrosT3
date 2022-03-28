using Microsoft.EntityFrameworkCore;
using WebApiCarros1.Entidades;

namespace WebApiCarros1
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Carro> Carros { get; set; }
        public DbSet<Clase> Clases { get; set; }
    }
}
