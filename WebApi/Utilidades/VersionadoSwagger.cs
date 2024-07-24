using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace WebApi.Utilidades
{
    public class VersionadoSwagger : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceController = controller.ControllerType.Namespace; // controllers.v1
            var versionAPI = namespaceController.Split(".").Last().ToLower(); //v1
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
