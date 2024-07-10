using WebApi.Validaciones;

namespace WebApi.Entidades
{
    public class Libro
    {
        public int ID { get; set; }
        public int AutorID { get; set; }
        public Autor autor { get; set; }
        [ValidacionesNombre]
        public string Titulo { get; set; }
    }
}
