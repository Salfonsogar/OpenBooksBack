using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class LibroRepository : GenericRepository<Libro>, ILibroRepository
    {
        public LibroRepository(OpenBooksContext context)
            : base(context)
        {
        }
        public async Task<Libro?> GetByIdWithCategoriasAsync(int id)
        {
            return await _context.Libros
                .Include(l => l.LibroCategorias)
                    .ThenInclude(lc => lc.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Libro?> GetLibroCompletoAsync(int id)
        {
            return await Query(
                l => l.Id == id,
                l => l.LibroCategorias,
                l => l.Resenas,
                l => l.BibliotecaLibros,
                l => l.EstanteriaLibros
            )
            .FirstOrDefaultAsync();
        }
        public async Task<LibroDetailDto?> GetDetailAsync(int libroId)
        {
            return await _context.Libros
                .AsNoTracking()
                .Where(l => l.Id == libroId)
                .Select(l => new LibroDetailDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    Descripcion = l.Descripcion,
                    FechaPublicacion = l.FechaPublicacion,
                    ValoracionPromedio = l.ValoracionPromedio,
                    Portada = l.Portada,
                    Categorias = l.LibroCategorias.Select(lc => new LibroCategoriaDto
                    {
                        CategoriaId = lc.CategoriaId,
                        Nombre = lc.Categoria.Nombre
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
