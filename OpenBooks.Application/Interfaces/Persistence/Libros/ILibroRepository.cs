using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface ILibroRepository : IGenericRepository<Libro>
    {
        Task<Libro?> GetLibroCompletoAsync(int id);
        Task<Libro?> GetByIdWithCategoriasAsync(int id);
        Task<LibroDetailDto?> GetDetailAsync(int libroId);
    }
}
