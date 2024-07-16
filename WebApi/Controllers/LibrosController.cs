using AutoMapper;
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

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            //   var libro = await context.Libros.Include(x => x.Comentarios).FirstOrDefaultAsync(x => x.ID == id);
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.ID == id);

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
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

            return CreatedAtRoute("ObtenerLibro", new { id = libro.ID }, libroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int ID, LibroAltaDTO libroAltaDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.ID == ID);

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
    }
}
