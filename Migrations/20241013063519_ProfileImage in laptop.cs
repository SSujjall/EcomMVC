using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageinlaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2024, 10, 13), "$2a$11$G4DaemWgfzwHN8l3ohRNE.5nQAbiHVqWuT8TmfSIxLhaEEjhJVdrG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateOnly(2024, 10, 5), "$2a$11$.8neXflX6BXxvfmvV.sBEujBfKjJoueHFfVF1oJ6SUvAG1TGDcDKe" });
        }
    }
}
