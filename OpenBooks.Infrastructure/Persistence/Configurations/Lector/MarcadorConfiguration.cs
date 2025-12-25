using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Lector
{
    public class MarcadorConfiguration : IEntityTypeConfiguration<Marcador>
    {
        public void Configure(EntityTypeBuilder<Marcador> builder)
        {
            builder.ToTable("Marcadores");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Locator)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb");

            builder.Property(x => x.Href)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Progression)
                .HasPrecision(5, 4);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.LibroUsuarioId);
            builder.HasIndex(x => x.Href);
        }
    }
}
