using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenBooks.Api.Controllers.Auth;
using OpenBooks.Api.Middlewares;
using OpenBooks.Application.Extensions;
using OpenBooks.Infrastructure.Extensions;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//swagger & openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OpenBooks API",
        Version = "v1"
    });

    c.AddServer(new OpenApiServer
    {
        Url = "https://localhost:7163"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
//build app
var app = builder.Build();
//pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//middlewares global error handling
app.UseMiddleware<ExceptionMiddleware>();
//https redirection
app.UseHttpsRedirection();
//cors
app.UseCors("AllowAll");
//auth
app.UseAuthentication();
app.UseAuthorization();
//endpoints
app.MapControllers();
app.Run();