using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBuildingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_BuildingType_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingType",
                table: "BuildingType");

            migrationBuilder.RenameTable(
                name: "BuildingType",
                newName: "BuildingTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingTypes",
                table: "BuildingTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_BuildingTypes_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "BuildingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_BuildingTypes_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingTypes",
                table: "BuildingTypes");

            migrationBuilder.RenameTable(
                name: "BuildingTypes",
                newName: "BuildingType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingType",
                table: "BuildingType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_BuildingType_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "BuildingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
