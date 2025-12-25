using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Lector
{
    public class LibroUsuarioConfiguration : IEntityTypeConfiguration<LibroUsuario>
    {
        public void Configure(EntityTypeBuilder<LibroUsuario> builder)
        {
            builder.ToTable("LibrosUsuarios");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CurrentLocator)
                .HasColumnType("jsonb");

            builder.Property(x => x.CurrentHref)
                .HasMaxLength(500);

            builder.Property(x => x.Progression)
                .HasPrecision(5, 4);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

            builder.HasIndex(x => new { x.UsuarioId, x.LibroId })
                .IsUnique();

            builder.HasIndex(x => x.CurrentHref);

            builder.HasOne(x => x.Libro)
                .WithMany()
                .HasForeignKey(x => x.LibroId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Marcadores)
                .WithOne(m => m.LibroUsuario)
                .HasForeignKey(m => m.LibroUsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Resaltados)
                .WithOne(r => r.LibroUsuario)
                .HasForeignKey(r => r.LibroUsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
