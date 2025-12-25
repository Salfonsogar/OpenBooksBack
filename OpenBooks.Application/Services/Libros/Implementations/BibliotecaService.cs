using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Domain.Entities.Libros;

namespace OpenBooks.Application.Services.Libros.Implementations
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IUnitOfWork _unit;

        public BibliotecaService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result> AddLibroAsync(int usuarioId, int libroId)
        {
            try
            {
                var libro = await _unit.Libros.GetByIdAsync(libroId);
                if (libro == null)
                    return Result.Failure("El libro no existe");

                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                {
                    biblioteca = new Biblioteca
                    {
                        UsuarioId = usuarioId
                    };
                    await _unit.Bibliotecas.AddAsync(biblioteca);
                    await _unit.CommitAsync(); 
                }

                var existente = await _unit.BibliotecaLibros.GetByIdsAsync(biblioteca.Id, libroId);
                if (existente != null)
                    return Result.Failure("El libro ya está en la biblioteca");

                var bl = new BibliotecaLibro
                {
                    BibliotecaId = biblioteca.Id,
                    LibroId = libroId
                };

                await _unit.BibliotecaLibros.AddAsync(bl);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al agregar libro a la biblioteca: {ex.Message}");
            }
        }

        public async Task<Result> RemoveLibroAsync(int usuarioId, int libroId)
        {
            try
            {
                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                    return Result.Failure("Biblioteca no encontrada");

                var bl = await _unit.BibliotecaLibros.GetByIdsAsync(biblioteca.Id, libroId);
                if (bl == null)
                    return Result.Failure("El libro no se encuentra en la biblioteca");

                _unit.BibliotecaLibros.Remove(bl);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar libro de la biblioteca: {ex.Message}");
            }
        }

        public async Task<Result<BibliotecaDto>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                    return Result<BibliotecaDto>.Failure("Biblioteca no encontrada");

                var dto = new BibliotecaDto
                {
                    Id = biblioteca.Id,
                    UsuarioId = biblioteca.UsuarioId,
                    Libros = biblioteca.BibliotecaLibros?
                        .Select(bl => new LibroCardDto
                        {
                            Id = bl.Libro.Id,
                            Titulo = bl.Libro.Titulo,
                            Portada = bl.Libro.Portada,
                            ValoracionPromedio = bl.Libro.ValoracionPromedio
                        })
                        .ToList() ?? new List<LibroCardDto>()
                };

                return Result<BibliotecaDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<BibliotecaDto>.Failure($"Error al obtener la biblioteca: {ex.Message}");
            }
        }

        public async Task<Result> DeleteBibliotecaAsync(int usuarioId)
        {
            try
            {
                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                    return Result.Failure("Biblioteca no encontrada");

                if (biblioteca.BibliotecaLibros != null && biblioteca.BibliotecaLibros.Any())
                {
                    foreach (var bl in biblioteca.BibliotecaLibros.ToList())
                    {
                        _unit.BibliotecaLibros.Remove(bl);
                    }
                }

                _unit.Bibliotecas.Remove(biblioteca);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar la biblioteca: {ex.Message}");
            }
        }
    }
}
