using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Libro>> Get(int id)
        //{
        //    return await context.Libros.Include(x => x.autor).FirstOrDefaultAsync(x => x.ID == id);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post(Libro libro)
        //{

        //    var ExisteAutor = await context.Autors.AnyAsync(x => x.ID == libro.AutorID);
        //    if (!ExisteAutor)
        //    {
        //        return BadRequest($"El autor con id {libro.AutorID} no existe");
        //    }

        //    context.Add(libro);
        //    await context.SaveChangesAsync();
        //    return Ok(); 
        //}


    }
}
