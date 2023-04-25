using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileRecharge.Data.Migrations
{
    public partial class phonenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RechargeReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RechargeReports");
        }
    }
}
