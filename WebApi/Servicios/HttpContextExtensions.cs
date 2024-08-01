using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Servicios
{
    public static class HttpContextExtensions
    {
        public async static Task ParametroPaginacionHeader<T>(this HttpContext httpContext, IQueryable<T> query)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double cant = await query.CountAsync();
            httpContext.Response.Headers.Add("maxRegistros", cant.ToString());
        }
    }
}
