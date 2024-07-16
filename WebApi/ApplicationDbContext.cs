using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutorLibro>().
                HasKey(al => new { al.AutorID, al.LibroID });

        }

        public DbSet<Autor> Autors { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentarios> Comentarios{ get; set; }
        public DbSet<AutorLibro> AutoresLibros{ get; set; }
    }
}
