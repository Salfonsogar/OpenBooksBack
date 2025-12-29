using System.Collections.Generic;

namespace OpenBooks.Application.DTOs.Libros
{
    public class BibliotecaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<LibroCardDto> Libros { get; set; } = new();
    }

    public class BibliotecaCreateDto
    {
        public int UsuarioId { get; set; }
    }
}
