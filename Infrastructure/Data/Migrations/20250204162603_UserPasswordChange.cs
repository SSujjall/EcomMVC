using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class UserPasswordChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordChangeOTP",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordChangeOTP", "PasswordHash" },
                values: new object[] { null, "$2a$11$bzX8qyDu8Dyjjb3dKhm3PeHqo25/yvrrlWkwhp8BCA39GLAVt7sra" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordChangeOTP",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$doL9gxBWS4Rx34W0SkQIxOtWTj9o76CuKTNnwVfbUaybBfnqL8C..");
        }
    }
}
