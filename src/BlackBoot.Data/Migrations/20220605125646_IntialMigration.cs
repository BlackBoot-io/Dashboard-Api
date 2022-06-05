using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class IntialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.CreateTable(
                name: "CrowdSaleSchedule",
                schema: "Base",
                columns: table => new
                {
                    CrowdSaleScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenForSale = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MinimumBuy = table.Column<int>(type: "int", nullable: false),
                    InvestmentGoal = table.Column<int>(type: "int", nullable: false),
                    BonusCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdSaleSchedule", x => x.CrowdSaleScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                schema: "Base",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Base",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: false),
                    FullName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Nationality = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    WithdrawalWallet = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthdayDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Target = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Message = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "Payment",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CrowdSaleScheduleId = table.Column<int>(type: "int", nullable: false),
                    Network = table.Column<byte>(type: "tinyint", nullable: false),
                    UsdtAmount = table.Column<int>(type: "int", nullable: false),
                    CryptoAmount = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenCount = table.Column<int>(type: "int", nullable: false),
                    BonusCount = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ConfirmDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WalletAddress = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    TxId = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_CrowdSaleSchedule_CrowdSaleScheduleId",
                        column: x => x.CrowdSaleScheduleId,
                        principalSchema: "Base",
                        principalTable: "CrowdSaleSchedule",
                        principalColumn: "CrowdSaleScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJwtToken",
                schema: "Base",
                columns: table => new
                {
                    UserJwtTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccessTokenHash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    AccessTokenExpiresTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    RefreshTokenExpiresTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJwtToken", x => x.UserJwtTokenId);
                    table.ForeignKey(
                        name: "FK_UserJwtToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "WalletPool",
                schema: "Base",
                columns: table => new
                {
                    WalletPoolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Network = table.Column<byte>(type: "tinyint", nullable: false),
                    Address = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletPool", x => x.WalletPoolId);
                    table.ForeignKey(
                        name: "FK_WalletPool_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                schema: "Base",
                table: "User",
                columns: new[] { "UserId", "BirthdayDate", "Email", "FullName", "Gender", "IsActive", "Nationality", "Password", "RegistrationDate", "WithdrawalWallet" },
                values: new object[] { new Guid("c41d18f0-1c4c-4123-b703-64d167d707b4"), null, "Admin@BlackBoot.io", "Admin", (byte)1, true, "", "SELEtxzRpGEVskq+ddvHykdlDA2P8hB/2UHoo0uquvc=", new DateTime(2022, 6, 5, 17, 26, 46, 365, DateTimeKind.Local).AddTicks(3595), null });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_Email",
                schema: "Base",
                table: "Subscription",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CrowdSaleScheduleId",
                schema: "Payment",
                table: "Transaction",
                column: "CrowdSaleScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                schema: "Payment",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Base",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_WithdrawalWallet",
                schema: "Base",
                table: "User",
                column: "WithdrawalWallet",
                unique: true,
                filter: "[WithdrawalWallet] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwtToken_UserId",
                schema: "Base",
                table: "UserJwtToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletPool_UserId",
                schema: "Base",
                table: "WalletPool",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Subscription",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "UserJwtToken",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "WalletPool",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "CrowdSaleSchedule",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");
        }
    }
}
