using System.Collections.Generic;

namespace WebApi.Entidades
{
    public class Autor
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
