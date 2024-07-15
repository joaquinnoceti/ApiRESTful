using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.Entidades
{
    public class Autor
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

    }
}
