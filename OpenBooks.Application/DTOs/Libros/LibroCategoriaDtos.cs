using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.DTOs.Libros
{
    public class LibroCategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
