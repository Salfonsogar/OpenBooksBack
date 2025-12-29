using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Domain.Entities.Libros;

namespace OpenBooks.Application.Services.Libros.Implementations
{
    public class EstanteriaService : IEstanteriaService
    {
        private readonly IUnitOfWork _unit;

        public EstanteriaService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<EstanteriaDto>> CreateAsync(int usuarioId, EstanteriaCreateDto dto)
        {
            try
            {
                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                {
                    // crear biblioteca si no existe
                    biblioteca = new Biblioteca { UsuarioId = usuarioId };
                    await _unit.Bibliotecas.AddAsync(biblioteca);
                    await _unit.CommitAsync();
                }

                var estanteria = new Estanteria
                {
                    Nombre = dto.Nombre,
                    BibliotecaId = biblioteca.Id
                };

                await _unit.Estanterias.AddAsync(estanteria);
                await _unit.CommitAsync();

                var result = new EstanteriaDto
                {
                    Id = estanteria.Id,
                    Nombre = estanteria.Nombre,
                    BibliotecaId = estanteria.BibliotecaId,
                    Libros = new List<LibroCardDto>()
                };

                return Result<EstanteriaDto>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<EstanteriaDto>.Failure($"Error al crear estantería: {ex.Message}");
            }
        }

        public async Task<Result<EstanteriaDto>> GetByIdAsync(int id)
        {
            try
            {
                var est = await _unit.Estanterias.GetByIdWithLibrosAsync(id);
                if (est == null)
                    return Result<EstanteriaDto>.Failure("Estantería no encontrada");

                var dto = new EstanteriaDto
                {
                    Id = est.Id,
                    Nombre = est.Nombre,
                    BibliotecaId = est.BibliotecaId,
                    Libros = est.EstanteriaLibros?
                        .Select(el => new LibroCardDto
                        {
                            Id = el.Libro.Id,
                            Titulo = el.Libro.Titulo,
                            Portada = el.Libro.Portada,
                            ValoracionPromedio = el.Libro.ValoracionPromedio
                        })
                        .ToList() ?? new List<LibroCardDto>()
                };

                return Result<EstanteriaDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<EstanteriaDto>.Failure($"Error al obtener estantería: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<EstanteriaDto>>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null)
                    return Result<IEnumerable<EstanteriaDto>>.Success(Enumerable.Empty<EstanteriaDto>());

                var estanterias = _unit.Estanterias.Query(e => e.BibliotecaId == biblioteca.Id).ToList();

                var list = estanterias.Select(e => new EstanteriaDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    BibliotecaId = e.BibliotecaId,
                    Libros = new List<LibroCardDto>()
                }).ToList();

                return Result<IEnumerable<EstanteriaDto>>.Success(list);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<EstanteriaDto>>.Failure($"Error al obtener estanterías: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(int usuarioId, int id, EstanteriaUpdateDto dto)
        {
            try
            {
                var est = await _unit.Estanterias.GetByIdAsync(id);
                if (est == null)
                    return Result.Failure("Estantería no encontrada");

                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null || est.BibliotecaId != biblioteca.Id)
                    return Result.Failure("No autorizado");

                est.Nombre = dto.Nombre;
                _unit.Estanterias.Update(est);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al actualizar estantería: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int usuarioId, int id)
        {
            try
            {
                var est = await _unit.Estanterias.GetByIdWithLibrosAsync(id);
                if (est == null)
                    return Result.Failure("Estantería no encontrada");

                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null || est.BibliotecaId != biblioteca.Id)
                    return Result.Failure("No autorizado");

                // eliminar relaciones EstanteriaLibro
                if (est.EstanteriaLibros != null && est.EstanteriaLibros.Any())
                {
                    foreach (var el in est.EstanteriaLibros.ToList())
                        _unit.EstanteriaLibros.Remove(el);
                }

                _unit.Estanterias.Remove(est);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar estantería: {ex.Message}");
            }
        }

        public async Task<Result> AddLibroAsync(int usuarioId, int estanteriaId, int libroId)
        {
            try
            {
                var est = await _unit.Estanterias.GetByIdAsync(estanteriaId);
                if (est == null)
                    return Result.Failure("Estantería no encontrada");

                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null || est.BibliotecaId != biblioteca.Id)
                    return Result.Failure("No autorizado");

                var libro = await _unit.Libros.GetByIdAsync(libroId);
                if (libro == null)
                    return Result.Failure("El libro no existe");

                var existente = await _unit.EstanteriaLibros.GetByIdsAsync(estanteriaId, libroId);
                if (existente != null)
                    return Result.Failure("El libro ya está en la estantería");

                var el = new EstanteriaLibro
                {
                    EstanteriaId = estanteriaId,
                    LibroId = libroId
                };

                await _unit.EstanteriaLibros.AddAsync(el);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al añadir libro a la estantería: {ex.Message}");
            }
        }

        public async Task<Result> RemoveLibroAsync(int usuarioId, int estanteriaId, int libroId)
        {
            try
            {
                var est = await _unit.Estanterias.GetByIdAsync(estanteriaId);
                if (est == null)
                    return Result.Failure("Estantería no encontrada");

                var biblioteca = await _unit.Bibliotecas.GetByUsuarioIdAsync(usuarioId);
                if (biblioteca == null || est.BibliotecaId != biblioteca.Id)
                    return Result.Failure("No autorizado");

                var el = await _unit.EstanteriaLibros.GetByIdsAsync(estanteriaId, libroId);
                if (el == null)
                    return Result.Failure("El libro no se encuentra en la estantería");

                _unit.EstanteriaLibros.Remove(el);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar libro de la estantería: {ex.Message}");
            }
        }
    }
}
