using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/libros/{LibroID:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int LibroID)
        {
            var ExisteLibro = await context.Libros.AnyAsync(x => x.ID == LibroID);
            if (!ExisteLibro)
            {
                return NotFound();
            }
            var comentarios = await context.Comentarios.Where(x => x.LibroID == LibroID).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int LibroID, ComentarioAltaDTO comentarioAltaDTO)
        {
            var ExisteLibro = await context.Libros.AnyAsync(x => x.ID == LibroID);
            if (!ExisteLibro)
                return NotFound();

            var comentario = mapper.Map<Comentarios>(comentarioAltaDTO);
            comentario.LibroID = LibroID;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
