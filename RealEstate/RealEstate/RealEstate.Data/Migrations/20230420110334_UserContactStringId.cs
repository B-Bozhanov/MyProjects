using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserContactStringId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserContactId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UsersContacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersContacts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserContactId",
                table: "Properties",
                column: "UserContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_UsersContacts_UserContactId",
                table: "Properties",
                column: "UserContactId",
                principalTable: "UsersContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_UsersContacts_UserContactId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "UsersContacts");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UserContactId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UserContactId",
                table: "Properties");
        }
    }
}
