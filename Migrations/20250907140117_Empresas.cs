using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class Empresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "SubCategorias",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Pessoas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Mensalidades",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Categorias",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Alunos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeFantasia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "char(11)", maxLength: 11, nullable: true),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    RazaoSocial = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Cep = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Uf = table.Column<string>(type: "char(2)", maxLength: 2, nullable: false),
                    Cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Bairro = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Endereco = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_EmpresaId",
                table: "SubCategorias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_EmpresaId",
                table: "Pessoas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensalidades_EmpresaId",
                table: "Mensalidades",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EmpresaId",
                table: "Categorias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_EmpresaId",
                table: "Alunos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_NomeFantasia",
                table: "Empresas",
                column: "NomeFantasia",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Empresas_EmpresaId",
                table: "Alunos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Empresas_EmpresaId",
                table: "Categorias",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensalidades_Empresas_EmpresaId",
                table: "Mensalidades",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_Empresas_EmpresaId",
                table: "Pessoas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Empresas_EmpresaId",
                table: "SubCategorias",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Empresas_EmpresaId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Empresas_EmpresaId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensalidades_Empresas_EmpresaId",
                table: "Mensalidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_Empresas_EmpresaId",
                table: "Pessoas");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Empresas_EmpresaId",
                table: "SubCategorias");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_EmpresaId",
                table: "SubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_EmpresaId",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Mensalidades_EmpresaId",
                table: "Mensalidades");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_EmpresaId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_EmpresaId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "SubCategorias");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Mensalidades");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Alunos");
        }
    }
}
