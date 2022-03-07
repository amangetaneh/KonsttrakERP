using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "TransferItems",
                newName: "SendRemark");

            migrationBuilder.AddColumn<string>(
                name: "ReceiveRemark",
                table: "TransferItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestRemark",
                table: "TransferItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveRemark",
                table: "TransferItems");

            migrationBuilder.DropColumn(
                name: "RequestRemark",
                table: "TransferItems");

            migrationBuilder.RenameColumn(
                name: "SendRemark",
                table: "TransferItems",
                newName: "Remark");
        }
    }
}
