using System.ComponentModel.DataAnnotations;

namespace OpenBooks.Api.Dtos.Libros
{
    public class LibroCreateRequest
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;
        [Required]
        public string Autor { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "La portada es obligatoria")]
        public IFormFile Portada { get; set; }
        [Required(ErrorMessage = "El archivo del libro es obligatorio")]
        public IFormFile Archivo { get; set; }
        [Required]
        public DateTime FechaPublicacion { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una categoría")]
        public ICollection<int> CategoriasIds { get; set; } = new List<int>();
    }
}
