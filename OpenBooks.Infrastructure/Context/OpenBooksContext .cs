

using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Auth;
using OpenBooks.Domain.Entities.Comentarios;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooks.Infrastructure.Configurations.Comentarios;
using OpenBooks.Infrastructure.Persistence.Configurations.Auth;
using OpenBooks.Infrastructure.Persistence.Configurations.Libros;
using OpenBooks.Infrastructure.Persistence.Configurations.Usuarios;

namespace OpenBooksBack.Infrastructure.Data
{
    public class OpenBooksContext : DbContext
    {
        public OpenBooksContext(DbContextOptions<OpenBooksContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Sancion> Sanciones { get; set; }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<LibroCategoria> LibroCategoria { get; set; }

        public DbSet<Biblioteca> Bibliotecas { get; set; }
        public DbSet<Estanteria> Estanterias { get; set; }
        public DbSet<EstanteriaLibro> EstanteriaLibros { get; set; }
        public DbSet<BibliotecaLibro> BibliotecaLibros { get; set; }

        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Sugerencia> Sugerencias { get; set; }
        public DbSet<Denuncia> Denuncias { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new SancionConfiguration());

            modelBuilder.ApplyConfiguration(new LibroConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new LibroCategoriaConfiguration());

            modelBuilder.ApplyConfiguration(new BibliotecaConfiguration());
            modelBuilder.ApplyConfiguration(new EstanteriaConfiguration());
            modelBuilder.ApplyConfiguration(new EstanteriaLibroConfiguration());
            modelBuilder.ApplyConfiguration(new BibliotecaLibroConfiguration());

            modelBuilder.ApplyConfiguration(new ResenaConfiguration());
            modelBuilder.ApplyConfiguration(new SugerenciaConfiguration());
            modelBuilder.ApplyConfiguration(new DenunciaConfiguration());

            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
