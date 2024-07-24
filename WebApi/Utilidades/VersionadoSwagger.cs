using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace WebApi.Utilidades
{
    public class VersionadoSwagger : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceController = controller.ControllerType.Namespace; // controllers.V1
            var versionAPI = namespaceController.Split(".").Last().ToUpper(); //V1
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
