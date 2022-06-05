using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class AddIsImportantNotiification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("10d714b5-eaca-4eda-842c-f4b6bc9ceb97"));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("b3e3963a-818d-4f1e-a55d-f058c201f74d"), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 5, 15, 17, 20, 749, DateTimeKind.Local).AddTicks(9924), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Base",
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("b3e3963a-818d-4f1e-a55d-f058c201f74d"));

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Notification");

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("10d714b5-eaca-4eda-842c-f4b6bc9ceb97"), "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 4, 20, 46, 44, 148, DateTimeKind.Local).AddTicks(7797), null });
        }
    }
}
