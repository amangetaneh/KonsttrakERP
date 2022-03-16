using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class test33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyReceived",
                table: "TransferItems");

            migrationBuilder.RenameColumn(
                name: "QtySent",
                table: "TransferItems",
                newName: "QtyApproved");

            migrationBuilder.AddColumn<string>(
                name: "ApproveRemark",
                table: "TransferItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveRemark",
                table: "TransferItems");

            migrationBuilder.RenameColumn(
                name: "QtyApproved",
                table: "TransferItems",
                newName: "QtySent");

            migrationBuilder.AddColumn<int>(
                name: "QtyReceived",
                table: "TransferItems",
                type: "int",
                nullable: true);
        }
    }
}
