using AutoMapper;
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
            CreateMap<LibroAltaDTO, Libro>();
            CreateMap<Libro, LibroDTO>();
            CreateMap<ComentarioAltaDTO, Comentarios>();
            CreateMap<Comentarios,ComentarioDTO>();
            
        }
    }
}
