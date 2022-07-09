using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class ChangeCryptoAmountDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<decimal>(
                name: "CryptoAmount",
                schema: "Payment",
                table: "Transaction",
                type: "decimal(21,9)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CryptoAmount",
                schema: "Payment",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,9)");

        }
    }
}
