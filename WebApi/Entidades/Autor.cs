using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.Entidades
{
    public class Autor
    {
        public int ID { get; set; }
        [Required (ErrorMessage = "El campo nombre es obligatorio.")] //(ErrorMessage = "El campo {0} es obligatorio.")
        [StringLength(maximumLength:10,ErrorMessage ="El campo {0} debe ser menor a {1} caracteres")]
        [ValidacionesNombre] //validacion personalizada x atributo
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
