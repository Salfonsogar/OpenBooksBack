using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class BibliotecaConfiguration : IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.ToTable("Bibliotecas");

            builder.HasKey(x => x.Id);

            builder
                .HasMany(x => x.Estanterias)
                .WithOne(x => x.Biblioteca)
                .HasForeignKey(x => x.BibliotecaId);
        }
    }
}
