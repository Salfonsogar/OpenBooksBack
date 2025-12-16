using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Usuarios
{
    public class SancionConfiguration : IEntityTypeConfiguration<Sancion>
    {
        public void Configure(EntityTypeBuilder<Sancion> builder)
        {
            builder.ToTable("Sanciones");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Descripcion)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(s => s.DuracionDias)
                   .IsRequired();

            builder.Property(s => s.Fecha)
                   .HasColumnType("timestamp without time zone")
                   .HasDefaultValueSql("NOW()");

            builder.HasOne(s => s.Usuario)
                   .WithMany(u => u.Sanciones)
                   .HasForeignKey(s => s.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
