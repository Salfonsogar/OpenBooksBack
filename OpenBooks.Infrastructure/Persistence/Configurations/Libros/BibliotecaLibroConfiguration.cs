using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class BibliotecaLibroConfiguration : IEntityTypeConfiguration<BibliotecaLibro>
    {
        public void Configure(EntityTypeBuilder<BibliotecaLibro> builder)
        {
            builder.ToTable("BibliotecaLibro");

            builder.HasKey(x => new { x.BibliotecaId, x.LibroId });
        }
    }
}
