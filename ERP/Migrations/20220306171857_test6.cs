using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class test6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    RequestStoreId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    CheckedById = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_CheckedById",
                        column: x => x.CheckedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Purchases_Employees_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Purchases_Stores_RequestStoreId",
                        column: x => x.RequestStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedItems",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QtyRequested = table.Column<int>(type: "int", nullable: false),
                    QtyApproved = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedItems", x => new { x.PurchaseId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_PurchasedItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PurchasedItems_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Receives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    ReceiveStoreId = table.Column<int>(type: "int", nullable: false),
                    DeliveredFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    DeliveredById = table.Column<int>(type: "int", nullable: false),
                    ReceivedById = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_DeliveredById",
                        column: x => x.DeliveredById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Receives_Employees_ReceivedById",
                        column: x => x.ReceivedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Receives_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Receives_Stores_ReceiveStoreId",
                        column: x => x.ReceiveStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ReceivedItems",
                columns: table => new
                {
                    ReceiveId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedItems", x => new { x.ReceiveId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_ReceivedItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReceivedItems_Receives_ReceiveId",
                        column: x => x.ReceiveId,
                        principalTable: "Receives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_ItemId",
                table: "PurchasedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ApprovedById",
                table: "Purchases",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CheckedById",
                table: "Purchases",
                column: "CheckedById");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_RequestedById",
                table: "Purchases",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_RequestStoreId",
                table: "Purchases",
                column: "RequestStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedItems_ItemId",
                table: "ReceivedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ApprovedById",
                table: "Receives",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_DeliveredById",
                table: "Receives",
                column: "DeliveredById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_PurchaseId",
                table: "Receives",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ReceivedById",
                table: "Receives",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_ReceiveStoreId",
                table: "Receives",
                column: "ReceiveStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ApprovedById",
                table: "Transfers",
                column: "ApprovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_DeliveredById",
                table: "Transfers",
                column: "DeliveredById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_ReceivedById",
                table: "Transfers",
                column: "ReceivedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Employees_RequestedById",
                table: "Transfers",
                column: "RequestedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_ReceiveStoreId",
                table: "Transfers",
                column: "ReceiveStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Stores_SendStoreId",
                table: "Transfers",
                column: "SendStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropTable(
                name: "PurchasedItems");

            migrationBuilder.DropTable(
                name: "ReceivedItems");

            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropTable(
                name: "Purchases");

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
    }
}
