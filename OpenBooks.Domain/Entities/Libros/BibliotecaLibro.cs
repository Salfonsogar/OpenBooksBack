using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{
    public class BibliotecaLibro
    {
        public int BibliotecaId { get; set; }
        public Biblioteca Biblioteca { get; set; }

        public int LibroId { get; set; }
        public Libro Libro { get; set; }
    }
}
