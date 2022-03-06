using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
