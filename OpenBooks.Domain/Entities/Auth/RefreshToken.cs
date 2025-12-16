using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Expira { get; set; }

        public bool EstaRevocado { get; set; }

        public bool EstaExpirado => DateTime.UtcNow >= Expira;
    }

}
