using OpenBooks.Infrastructure.Repository.Interfaces.Auth;
using OpenBooks.Infrastructure.Repository.Interfaces.Comentarios;
using OpenBooks.Infrastructure.Repository.Interfaces.Libros;
using OpenBooks.Infrastructure.Repository.Interfaces.Usuarios;

namespace OpenBooksBack.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRolRepository Roles { get; }
        ISancionRepository Sanciones { get; }

        IBibliotecaRepository Bibliotecas { get; }
        IBibliotecaLibroRepository BibliotecaLibros { get; }

        ICategoriaRepository Categorias { get; }
        ILibroRepository Libros { get; }

        ILibroCategoriaRepository LibroCategorias { get; }
        IEstanteriaRepository Estanterias { get; }
        IEstanteriaLibroRepository EstanteriaLibros { get; }

        IDenunciaRepository Denuncias { get; }
        IResenaRepository Resenas { get; }
        ISugerenciaRepository Sugerencias { get; }

        Task<int> CommitAsync();
    }
}
