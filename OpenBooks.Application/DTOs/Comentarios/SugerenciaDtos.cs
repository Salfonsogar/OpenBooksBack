using System;

namespace OpenBooks.Application.DTOs.Comentarios
{
    public class SugerenciaCreateDto
    {
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SugerenciaResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
