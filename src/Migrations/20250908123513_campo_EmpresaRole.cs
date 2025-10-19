using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kendo_londrina.Migrations
{
    /// <inheritdoc />
    public partial class campo_EmpresaRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmpresaRole",
                table: "AspNetUsers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpresaRole",
                table: "AspNetUsers");
        }
    }
}
