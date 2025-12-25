using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Lector
{
    public class Resaltado
    {
        public int Id { get; set; }

        public int LibroUsuarioId { get; set; }
        public string LocatorStart { get; set; } = string.Empty;
        public string LocatorEnd { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public decimal? Progression { get; set; }
        public string SelectedText { get; set; } = string.Empty;
        public string? Context { get; set; }

        public string? Note { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual LibroUsuario? LibroUsuario { get; set; }
    }
}
