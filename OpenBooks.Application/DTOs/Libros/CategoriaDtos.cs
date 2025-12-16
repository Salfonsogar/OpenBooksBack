using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.DTOs.Libros
{
    public class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
    public class CategoriaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
    public class CategoriaUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
    public class CategoriaPatchDto
    {
        public string? Nombre { get; set; }
    }
}
