using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtencionSeedMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "BuildingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Тухла" },
                    { 2, "Панел" },
                    { 3, "ЕПК" },
                    { 4, "ПК" },
                    { 5, "Гредоред" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_BuildingType_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "BuildingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_BuildingType_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingType",
                table: "BuildingType");

            migrationBuilder.DeleteData(
                table: "BuildingType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BuildingType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BuildingType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BuildingType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BuildingType",
                keyColumn: "Id",
                keyValue: 5);

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
    }
}
