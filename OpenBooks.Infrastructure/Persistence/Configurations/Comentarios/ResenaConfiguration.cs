using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Infrastructure.Configurations.Comentarios
{
    public class ResenaConfiguration : IEntityTypeConfiguration<Resena>
    {
        public void Configure(EntityTypeBuilder<Resena> builder)
        {
            builder.ToTable("Resenas");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Puntuacion)
                .IsRequired();

            builder.Property(r => r.Descripcion)
                .HasMaxLength(500);

            builder.Property(r => r.Fecha)
                .IsRequired();

            // 🔥 Relación: Reseña → Usuario (1:N)
            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Resenas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 Relación: Reseña → Libro (1:N)
            builder.HasOne(r => r.Libro)
                .WithMany(l => l.Resenas)
                .HasForeignKey(r => r.LibroId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 Regla: un usuario solo puede reseñar un libro una vez
            builder.HasIndex(r => new { r.UsuarioId, r.LibroId })
                .IsUnique();
        }
    }
}
