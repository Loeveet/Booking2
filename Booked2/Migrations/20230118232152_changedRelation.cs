using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booked2.Migrations
{
    public partial class changedRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_PersonId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PersonId",
                table: "Bookings",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_PersonId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PersonId",
                table: "Bookings",
                column: "PersonId");
        }
    }
}
