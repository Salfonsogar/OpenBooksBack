using System;

namespace OpenBooks.Application.DTOs.Comentarios
{
    public class ResenaCreateDto
    {
        public int UsuarioId { get; set; }
        public int LibroId { get; set; }
        public int Puntuacion { get; set; }
        public string? Descripcion { get; set; }
    }

    public class ResenaUpdateDto
    {
        public int Puntuacion { get; set; }
        public string? Descripcion { get; set; }
    }

    public class ResenaResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LibroId { get; set; }
        public int Puntuacion { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
