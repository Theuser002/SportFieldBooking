using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFieldBooking.Data.Migrations
{
    public partial class MakeBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BookingStatuses",
                newName: "Status");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartHour",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndHour",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "BookingStatuses",
                newName: "Name");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartHour",
                table: "Bookings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndHour",
                table: "Bookings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
