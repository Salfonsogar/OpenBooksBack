using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenBooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Portada = table.Column<byte[]>(type: "bytea", nullable: true),
                    Archivo = table.Column<byte[]>(type: "bytea", nullable: true),
                    ValoracionPromedio = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibroCategoria",
                schema: "public",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroCategoria", x => new { x.LibroId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_LibroCategoria_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "public",
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibroCategoria_Libros_LibroId",
                        column: x => x.LibroId,
                        principalSchema: "public",
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Genero = table.Column<string>(type: "text", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    RolId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "public",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId1",
                        column: x => x.RolId1,
                        principalSchema: "public",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bibliotecas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bibliotecas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bibliotecas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Denuncias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioDenuncianteId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioDenunciadoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denuncias_Usuarios_UsuarioDenunciadoId",
                        column: x => x.UsuarioDenunciadoId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denuncias_Usuarios_UsuarioDenuncianteId",
                        column: x => x.UsuarioDenuncianteId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denuncias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Expira = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstaRevocado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resenas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Puntuacion = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LibroId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resenas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resenas_Libros_LibroId",
                        column: x => x.LibroId,
                        principalSchema: "public",
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resenas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sanciones",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DuracionDias = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sanciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sugerencias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sugerencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sugerencias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BibliotecaLibro",
                schema: "public",
                columns: table => new
                {
                    BibliotecaId = table.Column<int>(type: "integer", nullable: false),
                    LibroId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibliotecaLibro", x => new { x.BibliotecaId, x.LibroId });
                    table.ForeignKey(
                        name: "FK_BibliotecaLibro_Bibliotecas_BibliotecaId",
                        column: x => x.BibliotecaId,
                        principalSchema: "public",
                        principalTable: "Bibliotecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BibliotecaLibro_Libros_LibroId",
                        column: x => x.LibroId,
                        principalSchema: "public",
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estanterias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BibliotecaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estanterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estanterias_Bibliotecas_BibliotecaId",
                        column: x => x.BibliotecaId,
                        principalSchema: "public",
                        principalTable: "Bibliotecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstanteriaLibro",
                schema: "public",
                columns: table => new
                {
                    EstanteriaId = table.Column<int>(type: "integer", nullable: false),
                    LibroId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstanteriaLibro", x => new { x.EstanteriaId, x.LibroId });
                    table.ForeignKey(
                        name: "FK_EstanteriaLibro_Estanterias_EstanteriaId",
                        column: x => x.EstanteriaId,
                        principalSchema: "public",
                        principalTable: "Estanterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstanteriaLibro_Libros_LibroId",
                        column: x => x.LibroId,
                        principalSchema: "public",
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BibliotecaLibro_LibroId",
                schema: "public",
                table: "BibliotecaLibro",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Bibliotecas_UsuarioId",
                schema: "public",
                table: "Bibliotecas",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_UsuarioDenunciadoId",
                schema: "public",
                table: "Denuncias",
                column: "UsuarioDenunciadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_UsuarioDenuncianteId_UsuarioDenunciadoId",
                schema: "public",
                table: "Denuncias",
                columns: new[] { "UsuarioDenuncianteId", "UsuarioDenunciadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_UsuarioId",
                schema: "public",
                table: "Denuncias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EstanteriaLibro_LibroId",
                schema: "public",
                table: "EstanteriaLibro",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Estanterias_BibliotecaId",
                schema: "public",
                table: "Estanterias",
                column: "BibliotecaId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroCategoria_CategoriaId",
                schema: "public",
                table: "LibroCategoria",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                schema: "public",
                table: "RefreshTokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenas_LibroId",
                schema: "public",
                table: "Resenas",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenas_UsuarioId_LibroId",
                schema: "public",
                table: "Resenas",
                columns: new[] { "UsuarioId", "LibroId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_UsuarioId",
                schema: "public",
                table: "Sanciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Sugerencias_UsuarioId",
                schema: "public",
                table: "Sugerencias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                schema: "public",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId1",
                schema: "public",
                table: "Usuarios",
                column: "RolId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibliotecaLibro",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Denuncias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EstanteriaLibro",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LibroCategoria",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Resenas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Sanciones",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Sugerencias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Estanterias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Libros",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Bibliotecas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "public");
        }
    }
}
