using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Auth;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Auth
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(rt => rt.UsuarioId)   
                   .IsRequired();

            builder.Property(rt => rt.Creado)
                   .IsRequired();

            builder.Property(rt => rt.Expira)
                   .IsRequired();

            builder.Property(rt => rt.EstaRevocado)
                   .IsRequired();

            builder.HasOne(rt => rt.Usuario)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
