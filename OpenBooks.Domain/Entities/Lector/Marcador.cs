using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Lector
{
    public class Marcador
    {
        public int Id { get; set; }

        public int LibroUsuarioId { get; set; }

        public string? Label { get; set; }
        public string Locator { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public decimal? Progression { get; set; }
        public string? Metadata { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual LibroUsuario? LibroUsuario { get; set; }
    }
}
