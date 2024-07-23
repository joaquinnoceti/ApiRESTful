using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApi.Servicios
{
    public class GeneradorLinks
    {
        private readonly IAuthorizationService authorizationServices;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;

        public GeneradorLinks(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            this.authorizationServices = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper ConstruirURLHelper()
        {
            var factoria = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factoria.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private async Task<bool> EsAdmin() 
        {
            var httpcontext = httpContextAccessor.HttpContext;
            var result = await authorizationServices.AuthorizeAsync(httpcontext.User, "esAdmin");

            return result.Succeeded;
        }

        public async Task GenerarEnlaces(AutorDTO autorDTO)
        {
            var esAdmin = await EsAdmin();
            var Url = ConstruirURLHelper();

            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutorID", new { ID = autorDTO.ID }), descripcion: "self", metodo: "GET"));
            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ActualizarAutor", new { ID = autorDTO.ID }), descripcion: "autor-actualizar", metodo: "PUT"));

                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("EliminarAutor", new { ID = autorDTO.ID }), descripcion: "autor-eliminar", metodo: "DELETE"));

            }

        }
    }
}
