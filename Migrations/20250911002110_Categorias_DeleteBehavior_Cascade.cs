using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class Categorias_DeleteBehavior_Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
