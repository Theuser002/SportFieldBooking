﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFieldBooking.Data.Migrations
{
    public partial class seed_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Balance", "Code", "Created", "Email", "IsActive", "Password", "RefreshToken", "Role", "Username", "refreshTokenExpiryTime" },
                values: new object[] { 420L, 1000000000000L, "admin", new DateTime(2022, 5, 30, 16, 26, 3, 406, DateTimeKind.Local).AddTicks(9852), "admin@gmail.com", true, "admin", null, 0, "admin", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 420L);
        }
    }
}
