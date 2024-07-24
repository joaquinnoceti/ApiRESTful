using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace WebApi.Utilidades
{
    public class VersionadoHeader : Attribute, IActionConstraint
    {
        private readonly string header;
        private readonly string valor;

        public VersionadoHeader(string header, string valor)
        {
            this.header = header;
            this.valor = valor;
        }
        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var headers = context.RouteContext.HttpContext.Request.Headers;

            if (!headers.ContainsKey(header))
                return false;

            return string.Equals(headers[header], valor, StringComparison.OrdinalIgnoreCase);

        }
    }
}
