using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class EstanteriaConfiguration : IEntityTypeConfiguration<Estanteria>
    {
        public void Configure(EntityTypeBuilder<Estanteria> builder)
        {
            builder.ToTable("Estanterias");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
