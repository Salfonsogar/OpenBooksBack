using OpenBooks.Domain.Entities.Usuarios;
using System;

namespace OpenBooks.Domain.Entities.Comentarios
{
    public class Denuncia
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int UsuarioDenuncianteId { get; set; }
        public Usuario UsuarioDenunciante { get; set; }
        public int UsuarioDenunciadoId { get; set; }
        public Usuario UsuarioDenunciado { get; set; }

    }
}
