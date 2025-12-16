using OpenBooks.Domain.Entities.Comentarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Libros
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public byte[]? Portada { get; set; }
        public byte[]? Archivo { get; set; }

        public decimal ValoracionPromedio { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public ICollection<LibroCategoria> LibroCategorias { get; set; } = new List<LibroCategoria>();
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();

        public ICollection<BibliotecaLibro> BibliotecaLibros { get; set; } = new List<BibliotecaLibro>();
        public ICollection<EstanteriaLibro> EstanteriaLibros { get; set; } = new List<EstanteriaLibro>();
    }
}
