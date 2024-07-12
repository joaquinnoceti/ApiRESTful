using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entidades;
using WebApi.Filtros;
using WebApi.Servicios;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
  //  [Authorize] 
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped, ServicioSingleton servicioSingleton,
            ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]  //retiene la informacion en cache x 10 segundos, optimiza recursos al no consultar otra vez la BBDD
        [Authorize]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGUIDS()
        {
            return Ok(new
            {
                AutoresController_Transient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),

                AutoresController_Scoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),

                AutoresController_Singleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton()
            });
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Obteniendo Autores...");
            //servicio.RealizarTarea();
            return await context.Autors.Include(x => x.Libros).ToListAsync();
        }


        [HttpGet("primero")]
        [ServiceFilter(typeof(FiltroDeAccion))]

        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            logger.LogInformation("Obteniendo Autores...");

            return await context.Autors.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> AutorxID(int id)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.ID == id);
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }
        [HttpGet("{nombre}/{param2?}")]   ///PARAMETRO 2 OPCIONAL
        public async Task<ActionResult<Autor>> AutorxNombre([FromHeader]string nombre,[FromQuery]int param2)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            var ExisteNombre = await context.Autors.AnyAsync(x => x.Nombre == autor.Nombre);

            if (ExisteNombre)
            {
                return BadRequest("Nombre de autor REPETIDO");
            }
            else
            {
                context.Add(autor);
                await context.SaveChangesAsync();
                return Ok();
            }
            
        }

        [HttpPut("{id:int}")]//api/autores/1(id)
        public async Task<ActionResult> Put(Autor autor,int id)
        {
            if(autor.ID != id)
            {
                return BadRequest("ID invalido");
            }

            var existe = await context.Autors.AnyAsync(x => x.ID == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autors.AnyAsync(x => x.ID == id);
            
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Autor {ID = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
