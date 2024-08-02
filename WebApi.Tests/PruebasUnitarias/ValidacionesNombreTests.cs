using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.Tests.PruebasUnitarias
{
    [TestClass]
    public class ValidacionesNombreTests
    {
        [TestMethod]
        public void PrimeraLetaMinuscula_DaError()
        {
            //Preparacion
            var validacionesNombre = new ValidacionesNombre();
            var valor = "joaco";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var result = validacionesNombre.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.AreEqual("Error, primera letra debe ser mayuscula",result.ErrorMessage);
        }
        [TestMethod]
        public void ValorNulo_NoDaError()
        {
            //Preparacion
            var validacionesNombre = new ValidacionesNombre();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var result = validacionesNombre.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ValorMayuscula_NoDaError()
        {
            //Preparacion
            var validacionesNombre = new ValidacionesNombre();
            string valor = "Jjacinto";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var result = validacionesNombre.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(result);
        }

    }
}