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
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.ID == id);

            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroAltaDTO libroAltaDTO)
        {

            //var ExisteAutor = await context.Autors.AnyAsync(x => x.ID == libro.AutorID);
            //if (!ExisteAutor)
            //{
            //    return BadRequest($"El autor con id {libro.AutorID} no existe");
            //}

            var libro = mapper.Map<Libro>(libroAltaDTO);
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
