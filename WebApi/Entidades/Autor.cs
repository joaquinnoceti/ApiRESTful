using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.Entidades
{
    public class Autor : IValidatableObject
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio.")] //(ErrorMessage = "El campo {0} es obligatorio.") --VALIDACIONES POR ATRIBUTO
        [StringLength(maximumLength: 10, ErrorMessage = "El campo {0} debe ser menor a {1} caracteres")]
   //     [ValidacionesNombre] //validacion personalizada x atributo
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var Letra = Nombre[0].ToString();
                if (Letra != Letra.ToUpper())
                    yield return new ValidationResult("La primer letra debe ser mayuscula",
                        new string[] { nameof(Nombre) });
            }
        }
    }
}
