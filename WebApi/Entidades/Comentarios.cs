using Microsoft.AspNetCore.Identity;

namespace WebApi.Entidades
{
    public class Comentarios
    {
        public int ID { get; set; }
        public string Mensaje { get; set; }
        public int LibroID { get; set; }
        public Libro Libro { get; set; }
        public string UsuarioID { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
