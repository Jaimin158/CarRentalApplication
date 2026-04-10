using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RebuildWithoutLocationForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Locations_LocationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_LocationId",
                table: "Vehicles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LocationId",
                table: "Vehicles",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Locations_LocationId",
                table: "Vehicles",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
