using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class ContaPagar_ContaReceber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContasPagar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "date", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Pago = table.Column<bool>(type: "bit", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "date", nullable: true),
                    MeioPagamento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ObsPagamento = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "date", nullable: true),
                    MotivoExclusao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PessoaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubCategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasPagar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasPagar_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasPagar_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasPagar_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasPagar_SubCategorias_SubCategoriaId",
                        column: x => x.SubCategoriaId,
                        principalTable: "SubCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContasReceber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "date", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Recebido = table.Column<bool>(type: "bit", nullable: false),
                    DataRecebimento = table.Column<DateTime>(type: "date", nullable: true),
                    MeioRecebimento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ObsRecebimento = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "date", nullable: true),
                    MotivoExclusao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PessoaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubCategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasReceber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasReceber_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasReceber_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasReceber_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContasReceber_SubCategorias_SubCategoriaId",
                        column: x => x.SubCategoriaId,
                        principalTable: "SubCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_CategoriaId",
                table: "ContasPagar",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_EmpresaId",
                table: "ContasPagar",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_PessoaId",
                table: "ContasPagar",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_SubCategoriaId",
                table: "ContasPagar",
                column: "SubCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_CategoriaId",
                table: "ContasReceber",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_EmpresaId",
                table: "ContasReceber",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_PessoaId",
                table: "ContasReceber",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_SubCategoriaId",
                table: "ContasReceber",
                column: "SubCategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContasPagar");

            migrationBuilder.DropTable(
                name: "ContasReceber");
        }
    }
}
