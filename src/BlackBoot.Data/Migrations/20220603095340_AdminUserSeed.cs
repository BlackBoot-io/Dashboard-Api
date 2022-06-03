using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class AdminUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Email", "FirstName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("9f3ba00e-85de-40c5-9d7a-d8192acad0ba"), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 3, 14, 23, 39, 824, DateTimeKind.Local).AddTicks(4727), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("9f3ba00e-85de-40c5-9d7a-d8192acad0ba"));
        }
    }
}
