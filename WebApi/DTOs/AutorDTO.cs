﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class AutorDTO : Recurso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }
}
