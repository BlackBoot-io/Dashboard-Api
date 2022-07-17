using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoot.Data.Migrations
{
    public partial class ChangeCrowdSaleScheduleDecimalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "Base",
                table: "CrowdSaleSchedule",
                type: "decimal(21,9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentIncreaseRate",
                schema: "Base",
                table: "CrowdSaleSchedule",
                type: "decimal(21,9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "Base",
                table: "CrowdSaleSchedule",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentIncreaseRate",
                schema: "Base",
                table: "CrowdSaleSchedule",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,9)");
        }
    }
}
