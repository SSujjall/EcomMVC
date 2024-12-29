using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class JustTesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2024, 12, 29), "$2a$11$whshpGOWygYGua5hKMKaXubrTqZxSwlPrLYxoqVQN5nNkaoViniJO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2024, 12, 25), "$2a$11$2W8SlQVRAKim24clsDBIROrzlmSFyt2teJZvIGQ6CYcUmS921Z6HC" });
        }
    }
}
