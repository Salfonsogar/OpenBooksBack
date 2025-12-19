using OpenBooks.Application.Interfaces.Persistence.Usuarios;
using OpenBooks.Application.Interfaces.Persistence.Auth;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Interfaces.Persistence.Comentarios;

namespace OpenBooks.Application.Common
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
