using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class LibroConfiguration : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> builder)
        {
            builder.ToTable("Libros");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Autor)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Descripcion)
                   .HasMaxLength(2000);

            builder.Property(x => x.ValoracionPromedio)
                   .HasPrecision(3, 1); 

            builder.Property(x => x.FechaPublicacion)
                   .HasColumnType("timestamp without time zone")
                   .IsRequired();

            builder
                .HasMany(x => x.LibroCategorias)
                .WithOne(x => x.Libro)
                .HasForeignKey(x => x.LibroId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.BibliotecaLibros)
                .WithOne(x => x.Libro)
                .HasForeignKey(x => x.LibroId);

            builder
                .HasMany(x => x.EstanteriaLibros)
                .WithOne(x => x.Libro)
                .HasForeignKey(x => x.LibroId);
        }
    }
}
