using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Auth.Interfaces;
using OpenBooks.Domain.Entities.Auth;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooks.Infrastructure.Services.Auth.Interfaces;
using OpenBooksBack.Infrastructure.UnitOfWork;

namespace OpenBooks.Application.Services.Auth.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioRegisterDto> _registerValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUnitOfWork unit,
            IMapper mapper,
            IValidator<UsuarioRegisterDto> registerValidator,
            IValidator<LoginRequestDto> loginValidator,
            IJwtService jwtService)
        {
            _unit = unit;
            _mapper = mapper;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtService = jwtService;
        }
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            var validationResult = await ValidateAsync(_loginValidator, dto);
            if (!validationResult.IsSuccess)
                return Result<LoginResponseDto>.Failure(validationResult.Error!);

            var usuarioResult = await AuthenticateAsync(dto);
            if (!usuarioResult.IsSuccess)
                return Result<LoginResponseDto>.Failure(usuarioResult.Error!);

            var tokens = CreateTokens(usuarioResult.Data!);
            await PersistRefreshTokenAsync(tokens.RefreshToken);

            return Result<LoginResponseDto>.Success(
                BuildLoginResponse(usuarioResult.Data!, tokens)
            );
        }
        public async Task<Result<UsuarioResponseDto>> RegisterAsync(UsuarioRegisterDto dto)
        {
            var validation = await ValidateAsync(_registerValidator, dto);

            if (await _unit.Usuarios.GetByEmailAsync(dto.Correo) != null)
                return Result<UsuarioResponseDto>.Failure("El email ya está registrado.");

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            var rol = await _unit.Roles.GetByNameAsync("User");
            if (rol == null)
                return Result<UsuarioResponseDto>.Failure("El rol por defecto no existe.");

            usuario.RolId = rol.Id;

            await _unit.Usuarios.AddAsync(usuario);
            await _unit.CommitAsync();

            return Result<UsuarioResponseDto>.Success(
                _mapper.Map<UsuarioResponseDto>(usuario)
            );
        }
        private async Task<Result<Usuario>> AuthenticateAsync(LoginRequestDto dto)
        {
            var usuario = await _unit.Usuarios
                .Query(u => u.Correo == dto.Correo, u => u.Rol)
                .FirstOrDefaultAsync();

            if (usuario == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena))
                return Result<Usuario>.Failure("Correo o contraseña inválidos.");

            return Result<Usuario>.Success(usuario);
        }
        private (string AccessToken, RefreshToken RefreshToken) CreateTokens(Usuario usuario)
        {
            var accessToken = _jwtService.GenerateAccessToken(
                usuario.Id,
                usuario.Correo
            );

            var refreshToken = new RefreshToken
            {
                Token = _jwtService.GenerateRefreshToken(),
                UsuarioId = usuario.Id,
                Creado = DateTime.UtcNow,
                Expira = DateTime.UtcNow.AddDays(7),
                EstaRevocado = false
            };

            return (accessToken, refreshToken);
        }
        private async Task PersistRefreshTokenAsync(RefreshToken token)
        {
            await _unit.RefreshTokens.AddAsync(token);
            await _unit.CommitAsync();
        }
        private LoginResponseDto BuildLoginResponse(Usuario usuario,(string AccessToken, RefreshToken RefreshToken) tokens)
        {
            var response = _mapper.Map<LoginResponseDto>(usuario);
            response.Token = tokens.AccessToken;
            response.RefreshToken = tokens.RefreshToken.Token;
            response.RefreshTokenExpires = tokens.RefreshToken.Expira;

            return response;
        }
        private async Task<Result> ValidateAsync<T>(IValidator<T> validator, T dto)
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return Result.Failure(validationResult.Errors.First().ErrorMessage);
            return Result.Success();
        }
    }
}
