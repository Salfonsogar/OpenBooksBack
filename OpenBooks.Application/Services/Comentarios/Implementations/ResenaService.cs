using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using OpenBooks.Application.Services.Comentarios.Interfaces;
using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Application.Services.Comentarios.Implementations
{
    public class ResenaService : IResenaService
    {
        private readonly IUnitOfWork _unit;
        private readonly IValidator<ResenaCreateDto> _createValidator;
        private readonly IValidator<ResenaUpdateDto> _updateValidator;

        public ResenaService(
            IUnitOfWork unit,
            IValidator<ResenaCreateDto> createValidator,
            IValidator<ResenaUpdateDto> updateValidator)
        {
            _unit = unit;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<ResenaResponseDto>> CreateAsync(ResenaCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<ResenaResponseDto>.Failure(validation.Errors.First().ErrorMessage);

            try
            {
                var existente = await _unit.Resenas.GetByUsuarioYLibro(dto.UsuarioId, dto.LibroId);
                if (existente != null)
                    return Result<ResenaResponseDto>.Failure("Ya existe una reseña de este usuario para el libro");

                var resena = new Resena
                {
                    UsuarioId = dto.UsuarioId,
                    LibroId = dto.LibroId,
                    Puntuacion = dto.Puntuacion,
                    Descripcion = dto.Descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _unit.Resenas.AddAsync(resena);
                await _unit.CommitAsync();

                await RecalcularValoracionPromedio(dto.LibroId);

                var response = new ResenaResponseDto
                {
                    Id = resena.Id,
                    UsuarioId = resena.UsuarioId,
                    LibroId = resena.LibroId,
                    Puntuacion = resena.Puntuacion,
                    Descripcion = resena.Descripcion,
                    Fecha = resena.Fecha
                };

                return Result<ResenaResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<ResenaResponseDto>.Failure($"Error al crear reseña: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(int id, ResenaUpdateDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result.Failure(validation.Errors.First().ErrorMessage);

            try
            {
                var resena = await _unit.Resenas.GetByIdAsync(id);
                if (resena == null)
                    return Result.Failure("Reseña no encontrada");

                resena.Puntuacion = dto.Puntuacion;
                resena.Descripcion = dto.Descripcion;

                _unit.Resenas.Update(resena);
                await _unit.CommitAsync();

                await RecalcularValoracionPromedio(resena.LibroId);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al actualizar reseña: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var resena = await _unit.Resenas.GetByIdAsync(id);
                if (resena == null)
                    return Result.Failure("Reseña no encontrada");

                var libroId = resena.LibroId;

                _unit.Resenas.Remove(resena);
                await _unit.CommitAsync();

                await RecalcularValoracionPromedio(libroId);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar reseña: {ex.Message}");
            }
        }

        public async Task<Result<ResenaResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var s = await _unit.Resenas.GetByIdAsync(id);
                if (s == null)
                    return Result<ResenaResponseDto>.Failure("Reseña no encontrada");

                var dto = new ResenaResponseDto
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    LibroId = s.LibroId,
                    Puntuacion = s.Puntuacion,
                    Descripcion = s.Descripcion,
                    Fecha = s.Fecha,
                    NombreUsuario = s.Usuario?.NombreUsuario
                };

                return Result<ResenaResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<ResenaResponseDto>.Failure($"Error al obtener reseña: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<ResenaResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            try
            {
                var query = _unit.Resenas
                    .Query()
                    .OrderByDescending(r => r.Fecha)
                    .Select(r => new ResenaResponseDto
                    {
                        Id = r.Id,
                        UsuarioId = r.UsuarioId,
                        LibroId = r.LibroId,
                        Puntuacion = r.Puntuacion,
                        Descripcion = r.Descripcion,
                        Fecha = r.Fecha,
                        NombreUsuario = r.Usuario.NombreUsuario
                    });

                var paged = query.ToPagedResult(pagination.Page, pagination.PageSize);

                return Result<PagedResult<ResenaResponseDto>>.Success(paged);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ResenaResponseDto>>.Failure($"Error al listar reseñas: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ResenaResponseDto>>> GetByLibroIdAsync(int libroId)
        {
            try
            {
                var list = await _unit.Resenas.GetByLibroIdAsync(libroId);
                var dto = list.Select(r => new ResenaResponseDto
                {
                    Id = r.Id,
                    UsuarioId = r.UsuarioId,
                    LibroId = r.LibroId,
                    Puntuacion = r.Puntuacion,
                    Descripcion = r.Descripcion,
                    Fecha = r.Fecha,
                    NombreUsuario = r.Usuario?.NombreUsuario
                }).ToList();

                return Result<IEnumerable<ResenaResponseDto>>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ResenaResponseDto>>.Failure($"Error al obtener reseñas del libro: {ex.Message}");
            }
        }

        public async Task<Result<ResenaResponseDto>> GetByUsuarioAndLibroAsync(int usuarioId, int libroId)
        {
            try
            {
                var r = await _unit.Resenas.GetByUsuarioYLibro(usuarioId, libroId);
                if (r == null)
                    return Result<ResenaResponseDto>.Failure("No existe reseña");

                var dto = new ResenaResponseDto
                {
                    Id = r.Id,
                    UsuarioId = r.UsuarioId,
                    LibroId = r.LibroId,
                    Puntuacion = r.Puntuacion,
                    Descripcion = r.Descripcion,
                    Fecha = r.Fecha,
                    NombreUsuario = r.Usuario?.NombreUsuario
                };

                return Result<ResenaResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<ResenaResponseDto>.Failure($"Error al consultar reseña: {ex.Message}");
            }
        }

        private async Task RecalcularValoracionPromedio(int libroId)
        {
            var reseñas = await _unit.Resenas.GetByLibroIdAsync(libroId);

            var puntuaciones = reseñas
                .Select(r => (int?)r.Puntuacion)
                .ToList();

            decimal promedio = 0;
            if (puntuaciones.Any(v => v.HasValue))
            {
                promedio = Math.Round((decimal)puntuaciones.Where(v => v.HasValue).Average(v => v!.Value), 1);
            }

            var libro = await _unit.Libros.GetByIdAsync(libroId);
            if (libro != null)
            {
                libro.ValoracionPromedio = promedio;
                _unit.Libros.Update(libro);
                await _unit.CommitAsync();
            }
        }
    }
}
