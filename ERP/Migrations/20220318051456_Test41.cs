using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "IssueItems",
                newName: "RequestRemark");

            migrationBuilder.AddColumn<string>(
                name: "ApproveRemark",
                table: "IssueItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HandRemark",
                table: "IssueItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyApproved",
                table: "IssueItems",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveRemark",
                table: "IssueItems");

            migrationBuilder.DropColumn(
                name: "HandRemark",
                table: "IssueItems");

            migrationBuilder.DropColumn(
                name: "QtyApproved",
                table: "IssueItems");

            migrationBuilder.RenameColumn(
                name: "RequestRemark",
                table: "IssueItems",
                newName: "Remark");
        }
    }
}
