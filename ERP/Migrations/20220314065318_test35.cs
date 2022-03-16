using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class test35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transfers",
                newName: "TransferId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stores",
                newName: "StoreId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sites",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Receives",
                newName: "ReceiveId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Purchases",
                newName: "PurchaseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Maintenances",
                newName: "MaintenanceId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Issues",
                newName: "IssueId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserRoles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TransferId",
                table: "Transfers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Stores",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "Sites",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReceiveId",
                table: "Receives",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PurchaseId",
                table: "Purchases",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MaintenanceId",
                table: "Maintenances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "Issues",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "Id");
        }
    }
}
