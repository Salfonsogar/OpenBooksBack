using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface IEstanteriaRepository : IGenericRepository<Estanteria>
    {
        Task<Estanteria?> GetByIdWithLibrosAsync(int id);
    }
}
