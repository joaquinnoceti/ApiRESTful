using AutoMapper;
using System.Collections.Generic;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Utilidades
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AutorAltaDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));
            CreateMap<LibroAltaDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();
            CreateMap<ComentarioAltaDTO, Comentarios>();
            CreateMap<Comentarios, ComentarioDTO>();

        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var result = new List<LibroDTO>();

            if (autor.AutoresLibros == null) { return result; }

            foreach(var autorLibro in autor.AutoresLibros)
            {
                result.Add(new LibroDTO()
                {
                    ID= autorLibro.LibroID,
                    Titulo = autorLibro.Libro.Titulo
                });
            }

            return result;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var result = new List<AutorDTO>();

            if (libro.AutoresLibros == null) { return result; }

            foreach (var autorlibro in libro.AutoresLibros)
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

            if (libroAltaDTO.AutoresID == null)
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
