using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class AddUserAvatarAndSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_CrowdSaleSchedule_CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction");

            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("5e6cf75c-5f8b-4314-a2f7-ac447aac2b35"));

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                schema: "Base",
                table: "User",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                schema: "Base",
                table: "User",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Avatar", "BirthdayDate", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "PasswordSalt", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("822f1c27-599f-4ba0-813c-8a664c46548a"), null, new DateTime(2022, 7, 1, 12, 8, 24, 121, DateTimeKind.Local).AddTicks(2381), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", null, new DateTime(2022, 7, 1, 12, 8, 24, 121, DateTimeKind.Local).AddTicks(2349), null });

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_CrowdSaleSchedule_CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction",
                column: "CrowdSaleScheduleId",
                principalSchema: "Base",
                principalTable: "CrowdSaleSchedule",
                principalColumn: "CrowdSaleScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_CrowdSaleSchedule_CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction");

            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("822f1c27-599f-4ba0-813c-8a664c46548a"));

            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                schema: "Base",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "BirthdayDate", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("5e6cf75c-5f8b-4314-a2f7-ac447aac2b35"), new DateTime(2022, 6, 12, 15, 34, 1, 639, DateTimeKind.Local).AddTicks(4966), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 12, 15, 34, 1, 639, DateTimeKind.Local).AddTicks(4932), null });

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_CrowdSaleSchedule_CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction",
                column: "CrowdSaleScheduleId",
                principalSchema: "Base",
                principalTable: "CrowdSaleSchedule",
                principalColumn: "CrowdSaleScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
