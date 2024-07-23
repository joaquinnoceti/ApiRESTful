using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }



        [HttpGet(Name = "ObtenerAutores")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] bool HATEOAS=true)
        {
            var autores = await context.Autors.ToListAsync();
            var dtos = mapper.Map<List<AutorDTO>>(autores);
            if (HATEOAS)
            {
                var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
                dtos.ForEach(dto => GenerarEnlaces(dto, esAdmin.Succeeded));

                var result = new ColeccionDeRecursos<AutorDTO> { Valores = dtos };
                result.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "self", metodo: "GET"));
                if (esAdmin.Succeeded)
                    result.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("CrearAutor", new { }), descripcion: "crear-autor", metodo: "POST"));
                return Ok(result);
            }

            return Ok(dtos);
        }

        [HttpGet("{nombre}", Name = "ObtenerAutorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> AutorxID(string nombre)
        {
            var autores = await context.Autors.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}", Name = "ObtenerAutorID")]
        [AllowAnonymous]
        public async Task<ActionResult<AutorDTOConLibros>> AutorxID(int id)
        {
            var autor = await context.Autors
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorlibroDB => autorlibroDB.Libro)
                .FirstOrDefaultAsync(autorDB => autorDB.ID == id);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");


            if (autor == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<AutorDTOConLibros>(autor);

            GenerarEnlaces(dto, esAdmin.Succeeded);

            return dto;
        }

        private void GenerarEnlaces(AutorDTO autorDTO, bool esAdmin)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutorID", new { ID = autorDTO.ID }), descripcion: "self", metodo: "GET"));
            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ActualizarAutor", new { ID = autorDTO.ID }), descripcion: "autor-actualizar", metodo: "PUT"));

                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("EliminarAutor", new { ID = autorDTO.ID }), descripcion: "autor-eliminar", metodo: "DELETE"));

            }

        }


        [HttpPost(Name = "CrearAutor")]
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

            var AutorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("ObtenerAutor", new { id = autor.ID }, AutorDTO);

        }

        [HttpPut("{id:int}", Name = "ActualizarAutor")]//api/autores/1(id)
        public async Task<ActionResult> Put(AutorAltaDTO autorDTO, int id)
        {

            var existe = await context.Autors.AnyAsync(x => x.ID == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorDTO);
            autor.ID = id;

            context.Update(autor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "EliminarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autors.AnyAsync(x => x.ID == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Autor { ID = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
