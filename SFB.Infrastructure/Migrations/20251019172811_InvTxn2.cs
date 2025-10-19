using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvTxn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WareHouseDest",
                table: "IAW_InventoryTxn");

            migrationBuilder.DropForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseOri",
                table: "IAW_InventoryTxn");

            migrationBuilder.RenameColumn(
                name: "WarehouseOri",
                table: "IAW_InventoryTxn",
                newName: "WarehouseOriginId");

            migrationBuilder.RenameColumn(
                name: "WareHouseDest",
                table: "IAW_InventoryTxn",
                newName: "WarehouseDestId");

            migrationBuilder.RenameIndex(
                name: "IX_IAW_InventoryTxn_WarehouseOri",
                table: "IAW_InventoryTxn",
                newName: "IX_IAW_InventoryTxn_WarehouseOriginId");

            migrationBuilder.RenameIndex(
                name: "IX_IAW_InventoryTxn_WareHouseDest",
                table: "IAW_InventoryTxn",
                newName: "IX_IAW_InventoryTxn_WarehouseDestId");

            migrationBuilder.AddForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseDestId",
                table: "IAW_InventoryTxn",
                column: "WarehouseDestId",
                principalTable: "IAW_Warehouse",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseOriginId",
                table: "IAW_InventoryTxn",
                column: "WarehouseOriginId",
                principalTable: "IAW_Warehouse",
                principalColumn: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseDestId",
                table: "IAW_InventoryTxn");

            migrationBuilder.DropForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseOriginId",
                table: "IAW_InventoryTxn");

            migrationBuilder.RenameColumn(
                name: "WarehouseOriginId",
                table: "IAW_InventoryTxn",
                newName: "WarehouseOri");

            migrationBuilder.RenameColumn(
                name: "WarehouseDestId",
                table: "IAW_InventoryTxn",
                newName: "WareHouseDest");

            migrationBuilder.RenameIndex(
                name: "IX_IAW_InventoryTxn_WarehouseOriginId",
                table: "IAW_InventoryTxn",
                newName: "IX_IAW_InventoryTxn_WarehouseOri");

            migrationBuilder.RenameIndex(
                name: "IX_IAW_InventoryTxn_WarehouseDestId",
                table: "IAW_InventoryTxn",
                newName: "IX_IAW_InventoryTxn_WareHouseDest");

            migrationBuilder.AddForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WareHouseDest",
                table: "IAW_InventoryTxn",
                column: "WareHouseDest",
                principalTable: "IAW_Warehouse",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseOri",
                table: "IAW_InventoryTxn",
                column: "WarehouseOri",
                principalTable: "IAW_Warehouse",
                principalColumn: "WarehouseId");
        }
    }
}
