using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{
    public class Biblioteca
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Estanteria> Estanterias { get; set; } = new List<Estanteria>();
        public ICollection<BibliotecaLibro> BibliotecaLibros { get; set; } = new List<BibliotecaLibro>();
    }
}
