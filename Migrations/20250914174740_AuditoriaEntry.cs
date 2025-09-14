using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriaEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Entidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EntidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DadosAntes = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    DadosDepois = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaEntries");
        }
    }
}
