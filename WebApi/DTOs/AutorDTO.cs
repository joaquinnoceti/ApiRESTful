using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class AutorDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }
    }
}
