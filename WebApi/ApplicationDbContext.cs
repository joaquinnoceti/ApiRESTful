using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Autor> Autors { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}
