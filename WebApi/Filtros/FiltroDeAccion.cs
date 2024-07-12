using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApi.Filtros
{
    public class FiltroDeAccion : IActionFilter
    {
        private readonly ILogger<FiltroDeAccion> logger;

        public FiltroDeAccion(ILogger<FiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("En tiempo de ejecucion...");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Terminada la ejecucion...");

        }


    }
}
