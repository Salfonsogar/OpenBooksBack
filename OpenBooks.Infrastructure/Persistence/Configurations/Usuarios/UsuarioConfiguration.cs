using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Usuarios
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NombreUsuario)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Correo)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Contrasena)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.FechaNacimiento)
                    .HasColumnType("timestamp without time zone");



            builder.Property(x => x.Telefono)
                   .HasMaxLength(20);

            builder
                .HasOne(x => x.Rol)
                .WithMany()
                .HasForeignKey(x => x.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Biblioteca)
                .WithOne(x => x.Usuario)
                .HasForeignKey<Biblioteca>(x => x.UsuarioId);
        }
    }
}
