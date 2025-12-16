using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
