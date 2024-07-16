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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libro = await context.Libros.Include(x => x.Comentarios).FirstOrDefaultAsync(x => x.ID == id);

            return mapper.Map<LibroDTO>(libro);
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

            if(libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
