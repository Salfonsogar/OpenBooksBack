using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Persistence.Configurations.Libros
{
    public class EstanteriaLibroConfiguration : IEntityTypeConfiguration<EstanteriaLibro>
    {
        public void Configure(EntityTypeBuilder<EstanteriaLibro> builder)
        {
            builder.ToTable("EstanteriaLibro");

            builder.HasKey(x => new { x.EstanteriaId, x.LibroId });
        }
    }
}
