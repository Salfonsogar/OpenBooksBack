using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Usuarios
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    }
}
