using AutoMapper;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooks.Infrastructure.Services.Auth.Interfaces;
using OpenBooksBack.Infrastructure.UnitOfWork;

namespace OpenBooks.Application.Services.Usuarios.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IValidator<UsuarioCreateDto> _createValidator;
        private readonly IValidator<UsuarioUpdateDto> _updateValidator;
        private readonly IValidator<UsuarioUpdatePerfilDto> _updatePerfilValidator;

        public UsuarioService(
            IUnitOfWork unit,
            IMapper mapper,
            IJwtService jwtService,
            IValidator<UsuarioCreateDto> createValidator,
            IValidator<UsuarioUpdateDto> updateValidator,
            IValidator<UsuarioUpdatePerfilDto> updatePerfilValidator)
        {
            _unit = unit;
            _mapper = mapper;
            _jwtService = jwtService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _updatePerfilValidator = updatePerfilValidator;
        }
        public async Task<Result<UsuarioResponseDto>> CreateAsync(UsuarioCreateDto dto)
        {
            var validation = await ValidateAsync(_createValidator, dto);
            if (!validation.IsSuccess)
                return Result<UsuarioResponseDto>.Failure(validation.Error);

            if (await _unit.Usuarios.GetByEmailAsync(dto.Correo) != null)
                return Result<UsuarioResponseDto>.Failure("El email ya está registrado");

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            var rol = await _unit.Roles.GetByIdAsync(usuario.RolId);
            if (rol == null)
                return Result<UsuarioResponseDto>.Failure("Rol no válido");

            usuario.Rol = rol;

            await _unit.Usuarios.AddAsync(usuario);
            await _unit.CommitAsync();

            return Result<UsuarioResponseDto>.Success(
                _mapper.Map<UsuarioResponseDto>(usuario)
            );
        }
        public async Task<Result<IEnumerable<UsuarioResponseDto>>>GetAllAsync(PaginationParams pagination)
        {
            var skip = (pagination.Page - 1) * pagination.PageSize;

            var usuarios = await _unit.Usuarios
                .GetAllWithRolAsync(skip, pagination.PageSize);

            return Result<IEnumerable<UsuarioResponseDto>>.Success(
                _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios)
            );
        }

        public async Task<Result<UsuarioResponseDto>> GetByIdAsync(int id)
        {
            var usuario = await _unit.Usuarios.GetByIdWithRolAsync(id);
            if (usuario == null)
                return Result<UsuarioResponseDto>.Failure("Usuario no encontrado");

            return Result<UsuarioResponseDto>.Success(
                _mapper.Map<UsuarioResponseDto>(usuario)
            );
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var usuario = await _unit.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                return Result.Failure("Usuario no encontrado");

            _unit.Usuarios.Remove(usuario);
            await _unit.CommitAsync();

            return Result.Success();
        }
        public async Task<Result> PatchAsync(int id, UsuarioUpdateDto dto)
        {
            var validation = await ValidateAsync(_updateValidator, dto);
            if (!validation.IsSuccess)
                return validation;

            var usuario = await _unit.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                return Result.Failure("Usuario no encontrado");

            _mapper.Map(dto, usuario);
            _unit.Usuarios.Update(usuario);
            await _unit.CommitAsync();

            return Result.Success();
        }
        public async Task<Result> PatchPerfilAsync(int id, UsuarioUpdatePerfilDto dto)
        {
            var validation = await ValidateAsync(_updatePerfilValidator, dto);
            if (!validation.IsSuccess)
                return validation;

            var usuario = await _unit.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                return Result.Failure("Usuario no encontrado");

            _mapper.Map(dto, usuario);
            _unit.Usuarios.Update(usuario);
            await _unit.CommitAsync();

            return Result.Success();
        }
        private static async Task<Result> ValidateAsync<T>(IValidator<T> validator, T dto)
        {
            var result = await validator.ValidateAsync(dto);

            return result.IsValid
                ? Result.Success()
                : Result.Failure(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
