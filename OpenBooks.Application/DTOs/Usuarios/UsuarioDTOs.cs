using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.DTOs.Usuarios
{
    public class UsuarioCreateDto
    {
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }

        public int RolId { get; set; }
    }


    public class UsuarioUpdateDto
    {
        public string? NombreCompleto { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? Telefono { get; set; }

        public int? RolId { get; set; }
    }

    public class UsuarioUpdatePerfilDto
    {
        public string? NombreCompleto { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Genero { get; set; }
        public string? Telefono { get; set; }
    }

    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
    }
}
