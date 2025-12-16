using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Libros
{
    public interface IEstanteriaRepository : IGenericRepository<Estanteria>
    {
        Task<Estanteria?> GetByIdWithLibrosAsync(int id);
    }
}
