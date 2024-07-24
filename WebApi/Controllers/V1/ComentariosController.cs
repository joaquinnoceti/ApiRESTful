using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Controllers.V1
{
    [ApiController]
    [Route("api/V1/libros/{LibroID:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet(Name ="ObtenerComentarios")]
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

        [HttpGet("{id:int}",Name ="ObtenerComentarioID")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int ID)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(x => x.ID == ID);

            if(comentario == null) { return NotFound(); }

            return mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost(Name = "PostearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int LibroID, ComentarioAltaDTO comentarioAltaDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioID = usuario.Id;

            var ExisteLibro = await context.Libros.AnyAsync(x => x.ID == LibroID);
            if (!ExisteLibro)
                return NotFound();

            var comentario = mapper.Map<Comentarios>(comentarioAltaDTO);
            comentario.LibroID = LibroID;
            comentario.UsuarioID = usuarioID;

            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { id = comentario.ID, LibroID = LibroID }, comentarioDTO);
        }


        [HttpPut("{id:int}",Name = "EditarComentario")]
        public async Task<ActionResult> Put(ComentarioAltaDTO comentarioAltaDTO, int id, int LibroID)
        {
            var ExisteLibro = await context.Libros.AnyAsync(x => x.ID == LibroID);
            if (!ExisteLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(x => x.ID == id);
            if(!existeComentario)
            {
                return NotFound();
            }
            var comentario = mapper.Map<Comentarios>(comentarioAltaDTO);

            comentario.ID = id;
            comentario.LibroID=LibroID;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
