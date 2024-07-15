using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.Entidades
{
    public class Libro
    {
        public int ID { get; set; }
        [Required]
        public string Titulo { get; set; }
        public List<Comentarios> Comentarios { get; set; }
    }
}
