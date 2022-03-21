using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyIssued",
                table: "IssueItems",
                newName: "QtyRequested");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_StoreId",
                table: "Issues",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Stores_StoreId",
                table: "Issues",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Stores_StoreId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_StoreId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "QtyRequested",
                table: "IssueItems",
                newName: "QtyIssued");
        }
    }
}
