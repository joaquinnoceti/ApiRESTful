using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApi.Utilidades
{
    public class HATEOASFilterAttribute : ResultFilterAttribute
    {
        protected bool IncluyeHATEOAS(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;

            if(!RespuestaExitosa(result))
                return false;

            var cabecera = context.HttpContext.Request.Headers["incluirHATEOAS"];

            if(cabecera.Count == 0)
                return false;

            var valor = cabecera[0];

            if (!valor.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        private bool RespuestaExitosa(ObjectResult result)
        {
            if(result == null || result.Value == null)
                return false;

            if(result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2"))
                return false;

            return true;
        }
    }
}
