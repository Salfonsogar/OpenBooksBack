using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class LibroCategoriaConfiguration : IEntityTypeConfiguration<LibroCategoria>
    {
        public void Configure(EntityTypeBuilder<LibroCategoria> builder)
        {
            builder.ToTable("LibroCategoria");

            builder.HasKey(x => new { x.LibroId, x.CategoriaId });

            builder
                .HasOne(x => x.Libro)
                .WithMany(x => x.LibroCategorias)
                .HasForeignKey(x => x.LibroId);

            builder
                .HasOne(x => x.Categoria)
                .WithMany(x => x.LibroCategorias)
                .HasForeignKey(x => x.CategoriaId);
        }
    }
}
