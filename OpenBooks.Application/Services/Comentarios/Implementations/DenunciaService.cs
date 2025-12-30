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
    public class DenunciaService : IDenunciaService
    {
        private readonly IUnitOfWork _unit;
        private readonly IValidator<DenunciaCreateDto> _createValidator;

        public DenunciaService(IUnitOfWork unit, IValidator<DenunciaCreateDto> createValidator)
        {
            _unit = unit;
            _createValidator = createValidator;
        }

        public async Task<Result<DenunciaResponseDto>> CreateAsync(DenunciaCreateDto dto, int usuarioDenuncianteId)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<DenunciaResponseDto>.Failure(validation.Errors.First().ErrorMessage);

            if (usuarioDenuncianteId == dto.UsuarioDenunciadoId)
                return Result<DenunciaResponseDto>.Failure("No puedes denunciarte a ti mismo");

            try
            {
                var denunciado = await _unit.Usuarios.GetByIdAsync(dto.UsuarioDenunciadoId);
                if (denunciado == null)
                    return Result<DenunciaResponseDto>.Failure("Usuario denunciado no existe");

                // comprobar duplicado usando el repositorio (no EF Core desde Application)
                var realizadas = await _unit.Denuncias.GetDenunciasRealizadasPorUsuario(usuarioDenuncianteId);
                if (realizadas.Any(r => r.UsuarioDenunciadoId == dto.UsuarioDenunciadoId))
                    return Result<DenunciaResponseDto>.Failure("Ya has denunciado a este usuario");

                var denuncia = new Denuncia
                {
                    UsuarioDenuncianteId = usuarioDenuncianteId,
                    UsuarioDenunciadoId = dto.UsuarioDenunciadoId,
                    Descripcion = dto.Descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _unit.Denuncias.AddAsync(denuncia);
                await _unit.CommitAsync();

                var response = new DenunciaResponseDto
                {
                    Id = denuncia.Id,
                    UsuarioDenuncianteId = denuncia.UsuarioDenuncianteId,
                    UsuarioDenunciadoId = denuncia.UsuarioDenunciadoId,
                    Descripcion = denuncia.Descripcion,
                    Fecha = denuncia.Fecha,
                    DenuncianteNombreUsuario = (await _unit.Usuarios.GetByIdAsync(denuncia.UsuarioDenuncianteId))?.NombreUsuario,
                    DenunciadoNombreUsuario = denunciado?.NombreUsuario
                };

                return Result<DenunciaResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<DenunciaResponseDto>.Failure($"Error al crear denuncia: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id, int usuarioSolicitanteId)
        {
            try
            {
                var d = await _unit.Denuncias.GetByIdAsync(id);
                if (d == null)
                    return Result.Failure("Denuncia no encontrada");

                if (d.UsuarioDenuncianteId != usuarioSolicitanteId)
                    return Result.Failure("No autorizado");

                _unit.Denuncias.Remove(d);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar denuncia: {ex.Message}");
            }
        }

        public async Task<Result<DenunciaResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var d = await _unit.Denuncias.GetByIdAsync(id);
                if (d == null)
                    return Result<DenunciaResponseDto>.Failure("Denuncia no encontrada");

                var dto = new DenunciaResponseDto
                {
                    Id = d.Id,
                    UsuarioDenuncianteId = d.UsuarioDenuncianteId,
                    UsuarioDenunciadoId = d.UsuarioDenunciadoId,
                    Descripcion = d.Descripcion,
                    Fecha = d.Fecha,
                    DenuncianteNombreUsuario = (await _unit.Usuarios.GetByIdAsync(d.UsuarioDenuncianteId))?.NombreUsuario,
                    DenunciadoNombreUsuario = (await _unit.Usuarios.GetByIdAsync(d.UsuarioDenunciadoId))?.NombreUsuario
                };

                return Result<DenunciaResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<DenunciaResponseDto>.Failure($"Error al obtener denuncia: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<DenunciaResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            try
            {
                var query = _unit.Denuncias
                    .Query()
                    .OrderByDescending(d => d.Fecha)
                    .Select(d => new DenunciaResponseDto
                    {
                        Id = d.Id,
                        UsuarioDenuncianteId = d.UsuarioDenuncianteId,
                        UsuarioDenunciadoId = d.UsuarioDenunciadoId,
                        Descripcion = d.Descripcion,
                        Fecha = d.Fecha
                    });

                var paged = query.ToPagedResult(pagination.Page, pagination.PageSize);

                // rellenar nombres de usuario (se realiza por llamadas al repo; posible N+1)
                foreach (var item in paged.Items)
                {
                    item.DenuncianteNombreUsuario = (await _unit.Usuarios.GetByIdAsync(item.UsuarioDenuncianteId))?.NombreUsuario;
                    item.DenunciadoNombreUsuario = (await _unit.Usuarios.GetByIdAsync(item.UsuarioDenunciadoId))?.NombreUsuario;
                }

                return Result<PagedResult<DenunciaResponseDto>>.Success(paged);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<DenunciaResponseDto>>.Failure($"Error al listar denuncias: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<DenunciaResponseDto>>> GetByUsuarioDenunciadoAsync(int usuarioId)
        {
            try
            {
                var list = await _unit.Denuncias.GetDenunciasRecibidasPorUsuario(usuarioId);

                var dto = new List<DenunciaResponseDto>();
                foreach (var d in list)
                {
                    dto.Add(new DenunciaResponseDto
                    {
                        Id = d.Id,
                        UsuarioDenuncianteId = d.UsuarioDenuncianteId,
                        UsuarioDenunciadoId = d.UsuarioDenunciadoId,
                        Descripcion = d.Descripcion,
                        Fecha = d.Fecha,
                        DenuncianteNombreUsuario = (await _unit.Usuarios.GetByIdAsync(d.UsuarioDenuncianteId))?.NombreUsuario
                    });
                }

                return Result<IEnumerable<DenunciaResponseDto>>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DenunciaResponseDto>>.Failure($"Error al obtener denuncias del usuario: {ex.Message}");
            }
        }
    }
}
