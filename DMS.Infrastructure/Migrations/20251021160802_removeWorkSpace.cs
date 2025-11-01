using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeWorkSpace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkSpaceName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkSpaceName",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkSpaceName",
                table: "AspNetUsers",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                column: "WorkSpaceName",
                value: "AhmedWorkspace");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                column: "WorkSpaceName",
                value: "MahmoudWorkspace");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3",
                column: "WorkSpaceName",
                value: "AbdoWorkspace");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkSpaceName",
                table: "AspNetUsers",
                column: "WorkSpaceName",
                unique: true);
        }
    }
}
