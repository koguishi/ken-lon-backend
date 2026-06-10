using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class AddResponsavel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponsavelId",
                table: "Mensalidades",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsavelId",
                table: "Alunos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Responsavel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsavel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsavel_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensalidades_ResponsavelId",
                table: "Mensalidades",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_ResponsavelId",
                table: "Alunos",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsavel_EmpresaId",
                table: "Responsavel",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Responsavel_ResponsavelId",
                table: "Alunos",
                column: "ResponsavelId",
                principalTable: "Responsavel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensalidades_Responsavel_ResponsavelId",
                table: "Mensalidades",
                column: "ResponsavelId",
                principalTable: "Responsavel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Responsavel_ResponsavelId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensalidades_Responsavel_ResponsavelId",
                table: "Mensalidades");

            migrationBuilder.DropTable(
                name: "Responsavel");

            migrationBuilder.DropIndex(
                name: "IX_Mensalidades_ResponsavelId",
                table: "Mensalidades");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_ResponsavelId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Mensalidades");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Alunos");
        }
    }
}
