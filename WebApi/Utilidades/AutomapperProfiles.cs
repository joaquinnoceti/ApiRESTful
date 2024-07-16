using AutoMapper;
using System.Collections.Generic;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Utilidades
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AutorAltaDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<LibroAltaDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioAltaDTO, Comentarios>();
            CreateMap<Comentarios,ComentarioDTO>();
            

        }
        private List<AutorDTO> MapLibroDTOAutores(Libro libro,LibroDTO libroDTO)
        {
            var result = new List<AutorDTO>();

            if (libro.AutoresLibros == null) { return result; }

            foreach(var autorlibro in libro.AutoresLibros)
            {
                result.Add(new AutorDTO()
                {
                    ID = autorlibro.AutorID,
                    Nombre = autorlibro.Autor.Nombre
                });
            }

            return result;
        }
        private List<AutorLibro> MapAutoresLibros(LibroAltaDTO libroAltaDTO, Libro libro)
        {
            var result = new List<AutorLibro>();

            if(libroAltaDTO.AutoresID == null)
            {
                return result;
            }

            foreach (var autorID in libroAltaDTO.AutoresID)
            {
                result.Add(new AutorLibro() { AutorID = autorID });
            }


            return result;
        }
    }
}
