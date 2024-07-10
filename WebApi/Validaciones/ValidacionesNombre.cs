using System.ComponentModel.DataAnnotations;

namespace WebApi.Validaciones
{
    public class ValidacionesNombre :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            var letra = value.ToString()[0].ToString();

            if(letra != letra.ToUpper())
            {
                return new ValidationResult("Error, primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}
