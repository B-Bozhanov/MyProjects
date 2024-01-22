using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditImageDeleteUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteUrl",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteUrl",
                table: "Images");
        }
    }
}
