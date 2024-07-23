﻿using System.Collections.Generic;

namespace WebApi.DTOs
{
    public class ColeccionDeRecursos<T> : Recurso where T : Recurso 
    {
        public List<T> Valores { get; set; }
    }
}
