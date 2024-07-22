using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibroID")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            //   var libro = await context.Libros.Include(x => x.Comentarios).FirstOrDefaultAsync(x => x.ID == id);
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.ID == id);

            if(libro == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost(Name ="InsertarLibro")]
        public async Task<ActionResult> Post(LibroAltaDTO libroAltaDTO)
        {
            if (libroAltaDTO.AutoresID == null)
            {
                return BadRequest("Es necesario agregar un Autor al libro.");
            }


            var IDAutores = await context.Autors
                .Where(x => libroAltaDTO.AutoresID.Contains(x.ID)).Select(x => x.ID).ToListAsync();

            if (IDAutores.Count != libroAltaDTO.AutoresID.Count)
            {
                return BadRequest("Se ingreso un id de autor invalido.");
            }


            var libro = mapper.Map<Libro>(libroAltaDTO);

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("ObtenerLibroID", new { id = libro.ID }, libroDTO);
        }

        [HttpPut("{id:int}",Name ="EditarLibro")]
        public async Task<ActionResult> Put(int id, LibroAltaDTO libroAltaDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.ID == id);

            if (libroDB == null) { return NotFound(); }

            libroDB = mapper.Map(libroAltaDTO, libroDB);

            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }
        }

        [HttpPatch("{id:int}",Name ="PatchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var Libro = await context.Libros.FirstOrDefaultAsync(x => x.ID == id);

            if (Libro == null)
            {
                return NotFound();
            }

            var LibroDTO = mapper.Map<LibroPatchDTO>(Libro);

            patchDocument.ApplyTo(LibroDTO, ModelState);
            mapper.Map(LibroDTO, Libro);
            var Valido = TryValidateModel(LibroDTO);

            if (!Valido)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}",Name ="BorrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.ID == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Libro { ID = id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
