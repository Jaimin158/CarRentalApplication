using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleCode",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Toronto Branch" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Mississauga Branch" }
                });

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
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Locations_LocationId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_LocationId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleCode",
                table: "Vehicles",
                column: "VehicleCode",
                unique: true);
        }
    }
}
