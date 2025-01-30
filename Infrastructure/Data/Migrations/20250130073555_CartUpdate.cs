using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class CartUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Carts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2025, 1, 30), "$2a$11$Xm8DZAC/u52uzVRzh9YRZuOP7OlsmnHNJTN7SHz/bWmqvmQ0iMRo." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2024, 12, 29), "$2a$11$whshpGOWygYGua5hKMKaXubrTqZxSwlPrLYxoqVQN5nNkaoViniJO" });
        }
    }
}
