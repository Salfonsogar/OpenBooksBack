using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Comentarios
{
    public class Sugerencia
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
