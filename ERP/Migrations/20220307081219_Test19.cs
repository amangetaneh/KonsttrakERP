using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SentById",
                table: "Transfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SentById",
                table: "Transfers",
                column: "SentById");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_SentById",
                table: "Transfers",
                column: "SentById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_SentById",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SentById",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SentById",
                table: "Transfers");
        }
    }
}
