using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{
    public class EstanteriaLibro
    {
        public int EstanteriaId { get; set; }
        public Estanteria Estanteria { get; set; }

        public int LibroId { get; set; }
        public Libro Libro { get; set; }
    }
}
