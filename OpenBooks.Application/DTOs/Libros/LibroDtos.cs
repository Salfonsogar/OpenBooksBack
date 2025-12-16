using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.DTOs.Libros
{
    public class LibroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public byte[]? Portada { get; set; }
        public byte[]? Archivo { get; set; }

        public DateTime FechaPublicacion { get; set; }
        public ICollection<int> CategoriasIds { get; set; } = new List<int>();
    }

    public class LibroUpdateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public byte[]? Portada { get; set; }
        public byte[]? Archivo { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public ICollection<int> CategoriasIds { get; set; } = new List<int>();
    }
    public class LibroPatchDto
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }

        public byte[]? Portada { get; set; }

        public DateTime? FechaPublicacion { get; set; }
        public ICollection<int>? CategoriasIds { get; set; }
    }
    public class LibroDetailDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public byte[]? Portada { get; set; }

        public decimal ValoracionPromedio { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public ICollection<LibroCategoriaDto> Categorias { get; set; } = new List<LibroCategoriaDto>();
    }
    public class LibroCardDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public byte[]? Portada { get; set; }
        public decimal ValoracionPromedio { get; set; }
    }
}
