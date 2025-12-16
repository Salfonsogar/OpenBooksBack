using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Comentarios
{
    public class Resena
    {
        public int Id { get; set; }

        public int Puntuacion { get; set; } //1-5
        public string? Descripcion { get; set; } //opcional
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
