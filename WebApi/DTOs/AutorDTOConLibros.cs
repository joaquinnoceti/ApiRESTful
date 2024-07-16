using System.Collections.Generic;

namespace WebApi.DTOs
{
    public class AutorDTOConLibros : AutorDTO
    {
        public List<LibroDTO> Libros { get; set; }
    }
}
