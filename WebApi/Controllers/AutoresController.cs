using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        //public async Task<ActionResult<List<Autor>>> Get()
        //{
        //    return await context.Autors.Include(x => x.Libros).ToListAsync();
        //}
        public List<Autor> GetListaAutores()
        {
            return context.Autors.Include(x => x.Libros).ToList();
        }


        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
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
        public async Task<ActionResult<Autor>> AutorxNombre(string nombre,int param2)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
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
