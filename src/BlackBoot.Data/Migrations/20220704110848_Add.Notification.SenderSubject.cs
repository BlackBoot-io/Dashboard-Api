using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class AddNotificationSenderSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Notification",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Notification",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

                }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("e8778d8b-73d2-4bb6-a087-b3a34fa09094"));

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Notification");

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Avatar", "BirthdayDate", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "PasswordSalt", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("822f1c27-599f-4ba0-813c-8a664c46548a"), null, new DateTime(2022, 7, 1, 12, 8, 24, 121, DateTimeKind.Local).AddTicks(2381), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", null, new DateTime(2022, 7, 1, 12, 8, 24, 121, DateTimeKind.Local).AddTicks(2349), null });
        }
    }
}
