using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Infrastructure.Configurations.Comentarios
{
    public class SugerenciaConfiguration : IEntityTypeConfiguration<Sugerencia>
    {
        public void Configure(EntityTypeBuilder<Sugerencia> builder)
        {
            builder.ToTable("Sugerencias");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Fecha)
                .IsRequired();

            builder.HasOne(s => s.Usuario)
                .WithMany(u => u.Sugerencias)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
