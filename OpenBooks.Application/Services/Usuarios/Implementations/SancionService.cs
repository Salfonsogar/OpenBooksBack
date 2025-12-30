using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;
using OpenBooks.Domain.Entities.Usuarios;

namespace OpenBooks.Application.Services.Usuarios.Implementations
{
    public class SancionService : ISancionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IValidator<SancionCreateDto> _createValidator;
        private readonly IValidator<SancionUpdateDto> _updateValidator;

        public SancionService(
            IUnitOfWork unit,
            IValidator<SancionCreateDto> createValidator,
            IValidator<SancionUpdateDto> updateValidator)
        {
            _unit = unit;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<SancionResponseDto>> CreateAsync(SancionCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<SancionResponseDto>.Failure(validation.Errors.First().ErrorMessage);

            try
            {
                var sancion = new Sancion
                {
                    UsuarioId = dto.UsuarioId,
                    DuracionDias = dto.DuracionDias,
                    Descripcion = dto.Descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _unit.Sanciones.AddAsync(sancion);
                await _unit.CommitAsync();

                var res = new SancionResponseDto
                {
                    Id = sancion.Id,
                    UsuarioId = sancion.UsuarioId,
                    DuracionDias = sancion.DuracionDias,
                    Descripcion = sancion.Descripcion,
                    Fecha = sancion.Fecha
                };

                return Result<SancionResponseDto>.Success(res);
            }
            catch (Exception ex)
            {
                return Result<SancionResponseDto>.Failure($"Error al crear sanción: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(int id, SancionUpdateDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result.Failure(validation.Errors.First().ErrorMessage);

            try
            {
                var sancion = await _unit.Sanciones.GetByIdAsync(id);
                if (sancion == null)
                    return Result.Failure("Sanción no encontrada");

                sancion.DuracionDias = dto.DuracionDias;
                sancion.Descripcion = dto.Descripcion;

                _unit.Sanciones.Update(sancion);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al actualizar sanción: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var sancion = await _unit.Sanciones.GetByIdAsync(id);
                if (sancion == null)
                    return Result.Failure("Sanción no encontrada");

                _unit.Sanciones.Remove(sancion);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar sanción: {ex.Message}");
            }
        }

        public async Task<Result<SancionResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var s = await _unit.Sanciones.GetByIdAsync(id);
                if (s == null)
                    return Result<SancionResponseDto>.Failure("Sanción no encontrada");

                var dto = new SancionResponseDto
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    DuracionDias = s.DuracionDias,
                    Descripcion = s.Descripcion,
                    Fecha = s.Fecha
                };

                return Result<SancionResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<SancionResponseDto>.Failure($"Error al obtener sanción: {ex.Message}");
            }
        }

        public Task<Result<PagedResult<SancionResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            try
            {
                var query = _unit.Sanciones
                    .Query()
                    .OrderByDescending(s => s.Fecha)
                    .Select(s => new SancionResponseDto
                    {
                        Id = s.Id,
                        UsuarioId = s.UsuarioId,
                        DuracionDias = s.DuracionDias,
                        Descripcion = s.Descripcion,
                        Fecha = s.Fecha
                    });

                var paged = query.ToPagedResult(pagination.Page, pagination.PageSize);

                return Task.FromResult(Result<PagedResult<SancionResponseDto>>.Success(paged));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result<PagedResult<SancionResponseDto>>.Failure($"Error al listar sanciones: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<SancionResponseDto>>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var list = await _unit.Sanciones.GetSancionesUsuario(usuarioId);

                var dto = list.Select(s => new SancionResponseDto
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    DuracionDias = s.DuracionDias,
                    Descripcion = s.Descripcion,
                    Fecha = s.Fecha
                }).ToList();

                return Result<IEnumerable<SancionResponseDto>>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<SancionResponseDto>>.Failure($"Error al obtener sanciones del usuario: {ex.Message}");
            }
        }
    }
}
