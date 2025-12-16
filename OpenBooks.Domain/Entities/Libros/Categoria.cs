using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public ICollection<LibroCategoria> LibroCategorias { get; set; } = new List<LibroCategoria>();

    }
}
