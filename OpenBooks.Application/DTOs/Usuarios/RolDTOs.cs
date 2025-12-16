using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.DTOs.Usuarios
{
    public class RolCreateDto
    {
        public string Nombre { get; set; }
    }

    public class RolUpdateDto
    {
        public string Nombre { get; set; }
    }

    public class RolResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
