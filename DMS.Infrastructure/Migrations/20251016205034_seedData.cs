using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_FolderId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Folders");

            migrationBuilder.AddColumn<string>(
                name: "ParentFolderId",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-admin", null, "Admin", "ADMIN" },
                    { "role-user", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FName", "LName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalSize", "TwoFactorEnabled", "WorkSpaceName" },
                values: new object[,]
                {
                    { "user-1", 0, "Cairo", "b7c1a4d2-8b23-4b12-b5b4-abcdefabcdef", new DateTime(2025, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc), "ahmed@example.com", true, "Ahmed", "Fergany", false, null, "AHMED@EXAMPLE.COM", "AHMED.FERGANY", "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==", "01000000000", true, "e3c1c6d2-9d48-4a90-a9a4-1234567890ab", null, false, "AhmedWorkspace" },
                    { "user-2", 0, "Alex", "f0d3e78b-4c8f-464e-b78f-e3b56c1487cc", new DateTime(2025, 10, 2, 12, 0, 0, 0, DateTimeKind.Utc), "mahmoud@example.com", true, "Mahmoud", "Badr", false, null, "MAHMOUD@EXAMPLE.COM", "MAHMOUD.BADR", "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==", "01000000000", true, "6d89f59e-bbfa-49d0-8e2e-a1b1e872e24d", null, false, "MahmoudWorkspace" },
                    { "user-3", 0, "Menofyia", "ab21854a-bbdf-4342-a86c-421c1d932f28", new DateTime(2025, 10, 3, 12, 0, 0, 0, DateTimeKind.Utc), "abdo@example.com", true, "Abdo", "Ahmed", false, null, "ABDO@EXAMPLE.COM", "ABDO.AHMED", "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==", "01000000000", true, "a6bdc263-9d47-4fd8-b929-101e1a99d9af", null, false, "AbdoWorkspace" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "role-admin", "user-1" },
                    { "role-user", "user-2" },
                    { "role-user", "user-3" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "AddedAt", "IsStarred", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { "folder-root", new DateTime(2025, 10, 3, 12, 0, 0, 0, DateTimeKind.Utc), false, "Root Folder", "user-2", null },
                    { "folder-child", new DateTime(2025, 10, 3, 12, 5, 0, 0, DateTimeKind.Utc), false, "Child Folder", "user-2", "folder-root" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "AddedAt", "FilePath", "FileType", "FolderId", "IsStarred", "Name", "Size" },
                values: new object[] { "doc-1", new DateTime(2025, 10, 3, 12, 50, 0, 0, DateTimeKind.Utc), "/files/sample.pdf", "pdf", "folder-child", false, "SampleDoc", 1024 });

            migrationBuilder.InsertData(
                table: "SharedItem",
                columns: new[] { "Id", "AddedAt", "DocumentId", "FolderId", "PermissionLevel", "SharedByUserId", "SharedWithUserId" },
                values: new object[] { "share-1", new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, "folder-child", "Read", "user-2", "user-3" });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-admin", "user-1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-user", "user-2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-user", "user-3" });

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: "doc-1");

            migrationBuilder.DeleteData(
                table: "SharedItem",
                keyColumn: "Id",
                keyValue: "share-1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-user");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3");

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: "folder-child");

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: "folder-root");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "Folders");

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_FolderId",
                table: "Folders",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
