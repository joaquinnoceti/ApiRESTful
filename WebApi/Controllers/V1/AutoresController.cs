﻿using AutoMapper;
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
using WebApi.Utilidades;

namespace WebApi.Controllers.V1
{
    [ApiController]
    [Route("api/V1/autores")]
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



        [HttpGet(Name = "ObtenerAutoresV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autors.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
                      
        }

        [HttpGet("{nombre}", Name = "ObtenerAutorNombreV1")]
        public async Task<ActionResult<List<AutorDTO>>> AutorxID(string nombre)
        {
            var autores = await context.Autors.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}", Name = "ObtenerAutorIDV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> AutorxID(int id)
        {
            var autor = await context.Autors
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorlibroDB => autorlibroDB.Libro)
                .FirstOrDefaultAsync(autorDB => autorDB.ID == id);


            if (autor == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<AutorDTOConLibros>(autor);
            return dto;
        }




        [HttpPost(Name = "CrearAutorV1")]
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

            return CreatedAtRoute("ObtenerAutorV1", new { id = autor.ID }, AutorDTO);

        }

        [HttpPut("{id:int}", Name = "ActualizarAutorV1")]//api/autores/1(id)
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

        [HttpDelete("{id:int}", Name = "EliminarAutorV1")]
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