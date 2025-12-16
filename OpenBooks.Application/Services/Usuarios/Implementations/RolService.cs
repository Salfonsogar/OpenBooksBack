using AutoMapper;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooksBack.Infrastructure.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Usuarios.Implementations
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public RolService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Result<RolResponseDto>> CreateAsync(RolCreateDto dto)
        {
            var nombreUnico = await ValidarNombreUnicoAsync(dto.Nombre);
            if (!nombreUnico.IsSuccess)
                return Result<RolResponseDto>.Failure(nombreUnico.Error!);

            var rol = _mapper.Map<Rol>(dto);

            await _unit.Roles.AddAsync(rol);
            await _unit.CommitAsync();

            return Result<RolResponseDto>.Success(
                _mapper.Map<RolResponseDto>(rol)
            );
        }

        public async Task<Result<PagedResult<RolResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            var query = _unit.Roles
                .Query()
                .OrderBy(r => r.Id);

            var paged = query.ToPagedResult(pagination.Page, pagination.PageSize);

            return Result<PagedResult<RolResponseDto>>.Success(
                new PagedResult<RolResponseDto>
                {
                    Items = _mapper.Map<IEnumerable<RolResponseDto>>(paged.Items),
                    TotalItems = paged.TotalItems,
                    Page = paged.Page,
                    PageSize = paged.PageSize
                }
            );
        }

        public async Task<Result<RolResponseDto>> GetByIdAsync(int id)
        {
            var rol = await _unit.Roles.GetByIdAsync(id);

            if (rol == null)
                return Result<RolResponseDto>.Failure("Rol no encontrado");

            return Result<RolResponseDto>.Success(
                _mapper.Map<RolResponseDto>(rol)
            );
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var rol = await _unit.Roles.GetByIdAsync(id);
            if (rol == null)
                return Result.Failure("Rol no encontrado");

            _unit.Roles.Remove(rol);
            await _unit.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<RolResponseDto>> PatchAsync(int id, RolUpdateDto dto)
        {
            var rol = await _unit.Roles.GetByIdAsync(id);
            if (rol == null)
                return Result<RolResponseDto>.Failure("Rol no encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                var nombreUnico = await ValidarNombreUnicoAsync(dto.Nombre, id);
                if (!nombreUnico.IsSuccess)
                    return Result<RolResponseDto>.Failure(nombreUnico.Error!);
            }

            _mapper.Map(dto, rol);

            _unit.Roles.Update(rol);
            await _unit.CommitAsync();

            return Result<RolResponseDto>.Success(
                _mapper.Map<RolResponseDto>(rol)
            );
        }

        private async Task<Result> ValidarNombreUnicoAsync(string nombre, int? rolId = null)
        {
            var existente = await _unit.Roles.GetByNameAsync(nombre);

            if (existente != null && existente.Id != rolId)
                return Result.Failure("Ya existe un rol con ese nombre");

            return Result.Success();
        }
    }
}
