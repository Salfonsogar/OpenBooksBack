using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Infrastructure.Configurations.Comentarios
{
    public class DenunciaConfiguration : IEntityTypeConfiguration<Denuncia>
    {
        public void Configure(EntityTypeBuilder<Denuncia> builder)
        {
            builder.ToTable("Denuncias");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.Fecha)
                .IsRequired();

            builder.HasOne(d => d.UsuarioDenunciante)
                .WithMany(u => u.DenunciasRealizadas)
                .HasForeignKey(d => d.UsuarioDenuncianteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.UsuarioDenunciado)
                .WithMany(u => u.DenunciasRecibidas)
                .HasForeignKey(d => d.UsuarioDenunciadoId)
                .OnDelete(DeleteBehavior.Restrict);
            // un usuario no puede denunciar al mismo usuario mas de una vez
            builder.HasIndex(d => new { d.UsuarioDenuncianteId, d.UsuarioDenunciadoId })
                .IsUnique();
        }
    }
}
