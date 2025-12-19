using OpenBooks.Application.Common;

namespace OpenBooks.Application.DTOs.Libros
{
    public class LibroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public byte[] Portada { get; set; } = Array.Empty<byte>();
        public byte[] Archivo { get; set; } = Array.Empty<byte>();

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
    public class LibroSearchParams : PaginationParams
    {
        public string? Search { get; set; }  
        public int? CategoriaId { get; set; }
        public string? Autor { get; set; }
        public LibroOrderBy? OrderBy { get; set; }
    }
    public enum LibroOrderBy
    {
        TituloAsc,
        TituloDesc,
        ValoracionAsc,
        ValoracionDesc
    }

}
