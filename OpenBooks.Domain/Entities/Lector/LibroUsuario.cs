using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Domain.Entities.Lector
{
    public class LibroUsuario
    {
        public int Id { get; set; }

        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public string? CurrentLocator { get; set; }

        public string? CurrentHref { get; set; }
        public decimal? Progression { get; set; }

        public DateTime? LastReadAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public byte[]? RowVersion { get; set; }

        public virtual Libro? Libro { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Marcador> Marcadores { get; set; } = new List<Marcador>();
        public virtual ICollection<Resaltado> Resaltados { get; set; } = new List<Resaltado>();
    }
}
