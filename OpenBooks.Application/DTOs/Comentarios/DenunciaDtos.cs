using System;

namespace OpenBooks.Application.DTOs.Comentarios
{
    public class DenunciaCreateDto
    {
        public int UsuarioDenunciadoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class DenunciaResponseDto
    {
        public int Id { get; set; }
        public int UsuarioDenuncianteId { get; set; }
        public int UsuarioDenunciadoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? DenuncianteNombreUsuario { get; set; }
        public string? DenunciadoNombreUsuario { get; set; }
    }
}
