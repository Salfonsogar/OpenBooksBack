using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenBooks.Application.Common;
using OpenBooks.Application.Interfaces.Persistence.Auth;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Interfaces.Persistence.Usuarios;
using OpenBooks.Application.Interfaces.Services.Auth.Interfaces;
using OpenBooks.Application.Services.Lector;
using OpenBooks.Infrastructure.Repository.Implementations.Auth;
using OpenBooks.Infrastructure.Repository.Implementations.Libros;
using OpenBooks.Infrastructure.Repository.Implementations.Usuarios;
using OpenBooks.Infrastructure.Services.Auth;
using OpenBooks.Infrastructure.Services.Auth.Implementations;
using OpenBooks.Infrastructure.Services.Lector;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.UnitOfWork;


namespace OpenBooks.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            //DbContext
            services.AddDbContext<OpenBooksContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            //Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<OpenBooksContext>()
                .AddDefaultTokenProviders();

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<ISancionRepository, SancionRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ILibroCategoriaRepository, LibroCategoriaRepository>();
            services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
            services.AddScoped<IBibliotecaLibroRepository, BibliotecaLibroRepository>();
            services.AddScoped<IEstanteriaRepository, EstanteriaRepository>();
            services.AddScoped<IEstanteriaLibroRepository, EstanteriaLibroRepository>();

            //services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IEpubParser, EpubParser>();


            //jwt
            services.AddJwtAuth(config);

            //settings
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));

            return services;
        }
    }
}
