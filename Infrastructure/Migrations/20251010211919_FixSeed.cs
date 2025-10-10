using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id-123", 0, "820fb042-5e38-43a5-ab7e-ae385d9d2038", "richard.chalk@admin.se", true, false, null, "RICHARD.CHALK@ADMIN.SE", "RICHARD.CHALK@ADMIN.SE", "AQAAAAIAAYagAAAAEFEnfIYmMepxqNk1TkMD3Xz+cFIHlR4S+gxaHF+M3PMuzmalkCqZJj6Ut3a74lW/PA==", null, false, "9b3a3b2f-6d7c-4b0a-9f2d-3c2c1a1f0e9d", false, "richard.chalk@admin.se" });
        }
    }
}
