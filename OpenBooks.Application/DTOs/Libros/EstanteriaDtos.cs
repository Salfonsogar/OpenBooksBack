using System.Collections.Generic;

namespace OpenBooks.Application.DTOs.Libros
{
    public class EstanteriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int BibliotecaId { get; set; }
        public List<LibroCardDto> Libros { get; set; } = new();
    }

    public class EstanteriaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class EstanteriaUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
