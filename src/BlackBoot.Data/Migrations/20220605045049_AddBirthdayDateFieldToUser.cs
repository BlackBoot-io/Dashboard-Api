using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class AddBirthdayDateFieldToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("10d714b5-eaca-4eda-842c-f4b6bc9ceb97"));

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthdayDate",
                schema: "Base",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "BirthdayDate", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("d0a569cf-aa3d-4631-a365-cf4693441ba3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 5, 9, 20, 48, 975, DateTimeKind.Local).AddTicks(3616), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("d0a569cf-aa3d-4631-a365-cf4693441ba3"));

            migrationBuilder.DropColumn(
                name: "BirthdayDate",
                schema: "Base",
                table: "User");

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("10d714b5-eaca-4eda-842c-f4b6bc9ceb97"), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 4, 20, 46, 44, 148, DateTimeKind.Local).AddTicks(7797), null });
        }
    }
}
