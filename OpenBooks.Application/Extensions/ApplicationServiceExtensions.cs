using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OpenBooks.Application.Handlers.Auth;
using OpenBooks.Application.Handlers.Lector;
using OpenBooks.Application.Profiles.Libros;
using OpenBooks.Application.Profiles.Usuarios;
using OpenBooks.Application.Services.Auth;
using OpenBooks.Application.Services.Auth.Implementations;
using OpenBooks.Application.Services.Auth.Interfaces;
using OpenBooks.Application.Services.Libros.Implementations;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Application.Services.Usuarios.Implementations;
using OpenBooks.Application.Services.Usuarios.Interfaces;
using OpenBooks.Application.Validations.Auth;
using OpenBooks.Application.Validations.Libros;
using OpenBooks.Application.Validations.Usuarios;
using OpenBooks.Domain.Entities.Usuarios;

namespace OpenBooks.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper (todos los profiles)
            // usuarios
            services.AddAutoMapper(typeof(UsuarioProfile).Assembly);
            services.AddAutoMapper(typeof(RolProfile).Assembly);
            // libros
            services.AddAutoMapper(typeof(CategoriaProfile).Assembly);
            services.AddAutoMapper(typeof(LibroProfile).Assembly);

            // MediatR
            services.AddMediatR(cfg => { 
                cfg.RegisterServicesFromAssembly(typeof(GenerateRefreshTokenHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ForgotPasswordHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetBookManifestHandler).Assembly);
            });

            // Services
            // Auth 
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenRefreshService, TokenRefreshService>();
            // usuarios
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IRolService, RolService>();
            // Libros
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ILibroService, LibroService>();
            // comentarios

            // FluentValidation
            // usuarios
            services.AddValidatorsFromAssemblyContaining<UsuarioCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<UsuarioUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<UsuarioUpdatePerfilValidator>();
            services.AddValidatorsFromAssemblyContaining<RolCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<RolUpdateValidator>();
            // Auth
            services.AddValidatorsFromAssemblyContaining<GenerateRefreshTokenValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UsuarioRegisterValidator>();
            // libros
            services.AddValidatorsFromAssemblyContaining<CategoriaCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoriaPatchValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoriaUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<LibroCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<LibroUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<LibroPatchValidator>();

            return services;
        }
    }
}
