using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class Test7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_ApprovedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_CheckedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_RequestedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Stores_RequestStoreId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_ApprovedById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_DeliveredById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_ReceivedById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Purchases_PurchaseId",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Stores_ReceiveStoreId",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_DeliveredById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ReceivedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_RequestedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Stores_ReceiveStoreId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Stores_SendStoreId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_ApprovedById",
                table: "Purchases",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_CheckedById",
                table: "Purchases",
                column: "CheckedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_RequestedById",
                table: "Purchases",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Stores_RequestStoreId",
                table: "Purchases",
                column: "RequestStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_ApprovedById",
                table: "Receives",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_DeliveredById",
                table: "Receives",
                column: "DeliveredById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_ReceivedById",
                table: "Receives",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Purchases_PurchaseId",
                table: "Receives",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Stores_ReceiveStoreId",
                table: "Receives",
                column: "ReceiveStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_DeliveredById",
                table: "Transfers",
                column: "DeliveredById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ReceivedById",
                table: "Transfers",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_RequestedById",
                table: "Transfers",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_ReceiveStoreId",
                table: "Transfers",
                column: "ReceiveStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_SendStoreId",
                table: "Transfers",
                column: "SendStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_ApprovedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_CheckedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employees_RequestedById",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Stores_RequestStoreId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_ApprovedById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_DeliveredById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Employees_ReceivedById",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Purchases_PurchaseId",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Stores_ReceiveStoreId",
                table: "Receives");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_DeliveredById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_ReceivedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Employees_RequestedById",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Stores_ReceiveStoreId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Stores_SendStoreId",
                table: "Transfers");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_ApprovedById",
                table: "Purchases",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_CheckedById",
                table: "Purchases",
                column: "CheckedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employees_RequestedById",
                table: "Purchases",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Stores_RequestStoreId",
                table: "Purchases",
                column: "RequestStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_ApprovedById",
                table: "Receives",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_DeliveredById",
                table: "Receives",
                column: "DeliveredById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Employees_ReceivedById",
                table: "Receives",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Purchases_PurchaseId",
                table: "Receives",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Stores_ReceiveStoreId",
                table: "Receives",
                column: "ReceiveStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_DeliveredById",
                table: "Transfers",
                column: "DeliveredById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ReceivedById",
                table: "Transfers",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_RequestedById",
                table: "Transfers",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_ReceiveStoreId",
                table: "Transfers",
                column: "ReceiveStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_SendStoreId",
                table: "Transfers",
                column: "SendStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
