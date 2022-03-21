using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Sites_SiteId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "QtyHanded",
                table: "IssueItems");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "Maintenances",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_SiteId",
                table: "Maintenances",
                newName: "IX_Maintenances_StoreId");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedById",
                table: "Maintenances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "FixDate",
                table: "Maintenances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FixedById",
                table: "Maintenances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_FixedById",
                table: "Maintenances",
                column: "FixedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Employees_FixedById",
                table: "Maintenances",
                column: "FixedById",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Stores_StoreId",
                table: "Maintenances",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Employees_FixedById",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Stores_StoreId",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_FixedById",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "FixDate",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "FixedById",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Maintenances",
                newName: "SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_StoreId",
                table: "Maintenances",
                newName: "IX_Maintenances_SiteId");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedById",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyHanded",
                table: "IssueItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Sites_SiteId",
                table: "Maintenances",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
