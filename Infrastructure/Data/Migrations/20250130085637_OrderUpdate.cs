using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class OrderUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Orders",
                newName: "TotalOrderAmount");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "Orders",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalAmount",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$uRBF8Ty3GlWnSaJQMBgTYOUiWKCehU9Jyj66pIbpY04iv7T8PSUVG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "TotalOrderAmount",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Xm8DZAC/u52uzVRzh9YRZuOP7OlsmnHNJTN7SHz/bWmqvmQ0iMRo.");
        }
    }
}
