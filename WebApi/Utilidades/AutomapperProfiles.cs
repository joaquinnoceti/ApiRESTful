﻿using AutoMapper;
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
        }
    }
}
