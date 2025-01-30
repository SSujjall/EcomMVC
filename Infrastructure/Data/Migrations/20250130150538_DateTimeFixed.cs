using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$GP6uWq8ZeqBh41Fx3Q1ptuSpHeOQAfDdGO2eysX3DHRhNDPUB7cym");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Orders",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$eHr0Gaj2eWz/X1YyyPU/1ezjJwSk0y3N5Jz0dE254FOniiKkIdAlO");
        }
    }
}
