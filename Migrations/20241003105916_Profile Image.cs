using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$7FC.FKbydeeBiZSUfnkguuSIoRLGUyW.yteVPgKh6xJ/gjkPuusf6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "UserProfiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$opuD7lk7TUUoQWx7KQUpNufonti08WDli1W3PrtoeVF894Jxrrq3O");
        }
    }
}
