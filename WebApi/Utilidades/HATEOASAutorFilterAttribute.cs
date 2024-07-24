using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
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
            var autorDTO = resultado.Value as AutorDTO;
            if (autorDTO == null)
            {
                var autoresDTO = resultado.Value as List<AutorDTO> ?? throw new ArgumentException("Se espera instancia de AutorDTO o List<AutorDTO>");

                autoresDTO.ForEach(async autor => await generadorLinks.GenerarEnlaces(autor));
                resultado.Value = autoresDTO;
            }
            else
            {
                await generadorLinks.GenerarEnlaces(autorDTO);
            }
            await next();
        }
    }
}

