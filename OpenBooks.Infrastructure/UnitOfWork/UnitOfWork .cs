
using OpenBooks.Application.Common;
using OpenBooks.Application.Interfaces.Persistence.Auth;
using OpenBooks.Application.Interfaces.Persistence.Comentarios;
using OpenBooks.Application.Interfaces.Persistence.Lector;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Interfaces.Persistence.Usuarios;
using OpenBooks.Domain.Entities.Lector;
using OpenBooks.Infrastructure.Repository.Implementations.Auth;
using OpenBooks.Infrastructure.Repository.Implementations.Comentarios;
using OpenBooks.Infrastructure.Repository.Implementations.Lector;
using OpenBooks.Infrastructure.Repository.Implementations.Libros;
using OpenBooks.Infrastructure.Repository.Implementations.Usuarios;
using OpenBooksBack.Infrastructure.Data;

namespace OpenBooksBack.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OpenBooksContext _context;

        public IUsuarioRepository Usuarios { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IRolRepository Roles { get; }
        public ISancionRepository Sanciones { get; }

        public IBibliotecaRepository Bibliotecas { get; }
        public IBibliotecaLibroRepository BibliotecaLibros { get; }

        public ICategoriaRepository Categorias { get; }
        public ILibroRepository Libros { get; }
        public ILibroCategoriaRepository LibroCategorias { get; }

        public IEstanteriaRepository Estanterias { get; }
        public IEstanteriaLibroRepository EstanteriaLibros { get; }

        public IDenunciaRepository Denuncias { get; }
        public IResenaRepository Resenas { get; }
        public ISugerenciaRepository Sugerencias { get; }
        public ILibroUsuarioRepository LibroUsuarios { get; }
        public IMarcadorRepository Marcadores { get; }
        public IResaltadoRepository Resaltados { get; }

        public UnitOfWork(OpenBooksContext context)
        {
            _context = context;

            Usuarios = new UsuarioRepository(context);
            RefreshTokens = new RefreshTokenRepository(context);
            Roles = new RolRepository(context);
            Sanciones = new SancionRepository(context);

            Bibliotecas = new BibliotecaRepository(context);
            BibliotecaLibros = new BibliotecaLibroRepository(context);

            Categorias = new CategoriaRepository(context);
            Libros = new LibroRepository(context);
            LibroCategorias = new LibroCategoriaRepository(context);
            LibroUsuarios = new LibroUsuarioRepository(context);
            Marcadores = new MarcadorRepository(context);
            Resaltados = new ResaltadoRepository(context);

            Estanterias = new EstanteriaRepository(context);
            EstanteriaLibros = new EstanteriaLibroRepository(context);

            Denuncias = new DenunciaRepository(context);
            Resenas = new ResenaRepository(context);
            Sugerencias = new SugerenciaRepository(context);

        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
