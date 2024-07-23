using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Servicios;

namespace WebApi.Utilidades
{
    public class HATEOASAutorFilterAttribute : HATEOASFilterAttribute
    {
        private readonly GeneradorLinks generadorLinks;

        public HATEOASAutorFilterAttribute(GeneradorLinks generadorLinks)
        {
            this.generadorLinks = generadorLinks;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var Incluye = IncluyeHATEOAS(context);

            if (!Incluye)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;
            var modelo = resultado.Value as AutorDTO ?? throw new ArgumentNullException("Se espera instancia de AUTORDTO");
            await generadorLinks.GenerarEnlaces(modelo);
            await next();
        }
    }
}
