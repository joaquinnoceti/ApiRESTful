using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entidades;
using WebApi.Filtros;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autors.ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> AutorxID(string nombre)
        {
            var autores = await context.Autors.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            
            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> AutorxID(int id)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.ID == id);
            if (autor == null)
            {
                return NotFound();
            }
            return mapper.Map<AutorDTO>(autor);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorAltaDTO autorDTO)
        {
            var ExisteNombre = await context.Autors.AnyAsync(x => x.Nombre == autorDTO.Nombre);

            if (ExisteNombre)
            {
                return BadRequest("Nombre de autor REPETIDO");
            }

            var autor = mapper.Map<Autor>(autorDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();


        }

        [HttpPut("{id:int}")]//api/autores/1(id)
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.ID != id)
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
            context.Remove(new Autor { ID = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
