using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{

    public class Estanteria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public int BibliotecaId { get; set; }
        public Biblioteca Biblioteca { get; set; }
        public ICollection<EstanteriaLibro> EstanteriaLibros { get; set; } = new List<EstanteriaLibro>();
    }

}
