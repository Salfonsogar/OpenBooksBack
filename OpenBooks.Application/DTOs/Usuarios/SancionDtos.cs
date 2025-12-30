using System;

namespace OpenBooks.Application.DTOs.Usuarios
{
    public class SancionCreateDto
    {
        public int UsuarioId { get; set; }
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SancionUpdateDto
    {
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SancionResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
