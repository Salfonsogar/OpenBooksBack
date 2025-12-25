using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Lector
{
    public class ResaltadoConfiguration : IEntityTypeConfiguration<Resaltado>
    {
        public void Configure(EntityTypeBuilder<Resaltado> builder)
        {
            builder.ToTable("Resaltados");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.LocatorStart)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.LocatorEnd)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.Href)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Progression)
                .HasPrecision(5, 4);

            builder.Property(x => x.SelectedText)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Context)
                .HasColumnType("text");

            builder.Property(x => x.Note)
                .HasColumnType("text");

            builder.Property(x => x.Color)
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasIndex(x => x.LibroUsuarioId);
            builder.HasIndex(x => x.Href);

            builder.HasIndex(x => new { x.LibroUsuarioId, x.LocatorStart, x.LocatorEnd }).IsUnique();
        }
    }
}
