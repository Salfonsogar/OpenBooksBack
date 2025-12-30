using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenBooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuloLector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibroUsuarios",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LibroId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    CurrentLocator = table.Column<string>(type: "text", nullable: true),
                    CurrentHref = table.Column<string>(type: "text", nullable: true),
                    Progression = table.Column<decimal>(type: "numeric", nullable: true),
                    LastReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibroUsuarios_Libros_LibroId",
                        column: x => x.LibroId,
                        principalSchema: "public",
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibroUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "public",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marcadores",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LibroUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: true),
                    Locator = table.Column<string>(type: "text", nullable: false),
                    Href = table.Column<string>(type: "text", nullable: false),
                    Progression = table.Column<decimal>(type: "numeric", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marcadores_LibroUsuarios_LibroUsuarioId",
                        column: x => x.LibroUsuarioId,
                        principalSchema: "public",
                        principalTable: "LibroUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resaltados",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LibroUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    LocatorStart = table.Column<string>(type: "text", nullable: false),
                    LocatorEnd = table.Column<string>(type: "text", nullable: false),
                    Href = table.Column<string>(type: "text", nullable: false),
                    Progression = table.Column<decimal>(type: "numeric", nullable: true),
                    SelectedText = table.Column<string>(type: "text", nullable: false),
                    Context = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resaltados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resaltados_LibroUsuarios_LibroUsuarioId",
                        column: x => x.LibroUsuarioId,
                        principalSchema: "public",
                        principalTable: "LibroUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibroUsuarios_LibroId",
                schema: "public",
                table: "LibroUsuarios",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroUsuarios_UsuarioId",
                schema: "public",
                table: "LibroUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcadores_LibroUsuarioId",
                schema: "public",
                table: "Marcadores",
                column: "LibroUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Resaltados_LibroUsuarioId",
                schema: "public",
                table: "Resaltados",
                column: "LibroUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marcadores",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Resaltados",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LibroUsuarios",
                schema: "public");
        }
    }
}
