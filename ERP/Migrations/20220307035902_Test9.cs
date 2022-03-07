using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Employees_ReceivedById",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_ReceivedById",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "ReceivedById",
                table: "Issues",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "QtyTransfered",
                table: "IssueItems",
                newName: "QtyIssued");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "HandedById",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedById",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RequestedById",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_RequestedById",
                table: "Issues",
                column: "RequestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Employees_RequestedById",
                table: "Issues",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Employees_RequestedById",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_RequestedById",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Issues",
                newName: "ReceivedById");

            migrationBuilder.RenameColumn(
                name: "QtyIssued",
                table: "IssueItems",
                newName: "QtyTransfered");

            migrationBuilder.AlterColumn<int>(
                name: "HandedById",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedById",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ReceivedById",
                table: "Issues",
                column: "ReceivedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Employees_ReceivedById",
                table: "Issues",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
