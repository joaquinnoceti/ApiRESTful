﻿using System;
using System.Collections.Generic;

namespace WebApi.DTOs
{
    public class LibroDTO
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        // public List<ComentarioDTO> Comentarios { get; set; }
    }
}
