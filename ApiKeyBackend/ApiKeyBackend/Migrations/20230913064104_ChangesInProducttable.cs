using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiKeyBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInProducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorsPolicy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorsPolicy",
                table: "Products");
        }
    }
}
