﻿using System;
using System.Collections.Generic;

namespace WebApi.DTOs
{
    public class LibroAltaDTO
    {
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresID  { get; set; }
    }
}
