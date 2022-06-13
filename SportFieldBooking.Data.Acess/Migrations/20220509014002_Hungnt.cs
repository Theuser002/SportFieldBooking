using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFieldBooking.Data.Migrations
{
    public partial class Hungnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "BookingStatuses",
                newName: "StatusName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "BookingStatuses",
                newName: "Status");
        }
    }
}
