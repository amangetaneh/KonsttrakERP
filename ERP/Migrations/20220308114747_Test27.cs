using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Issues");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Transfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDate",
                table: "Transfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Transfers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "Transfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Maintenances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Maintenances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Issues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HandDate",
                table: "Issues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Issues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ReceiveDate",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "HandDate",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Issues");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transfers",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Maintenances",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Issues",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
