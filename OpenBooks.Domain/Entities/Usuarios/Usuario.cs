using System;
using System.Collections.Generic;
using System.Text;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Domain.Entities.Comentarios;
using OpenBooks.Domain.Entities.Auth;

namespace OpenBooks.Domain.Entities.Usuarios
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public Biblioteca Biblioteca { get; set; }
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public ICollection<Denuncia> Denuncias { get; set; } = new List<Denuncia>();
        public ICollection<Sugerencia> Sugerencias { get; set; } = new List<Sugerencia>();
        public ICollection<Denuncia> DenunciasRealizadas { get; set; } = new List<Denuncia>();
        public ICollection<Denuncia> DenunciasRecibidas { get; set; } = new List<Denuncia>();
        public ICollection<Sancion> Sanciones { get; set; } = new List<Sancion>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
