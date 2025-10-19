using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvTxn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IAW_InventoryTxn",
                columns: table => new
                {
                    TxnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnOrigin = table.Column<int>(type: "integer", nullable: true),
                    ModOrigin = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    WarehouseOri = table.Column<int>(type: "integer", nullable: true),
                    WareHouseDest = table.Column<int>(type: "integer", nullable: true),
                    StatusCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_InventoryTxn", x => x.TxnId);
                    table.ForeignKey(
                        name: "FK_IAW_InventoryTxn_IAW_Warehouse_WareHouseDest",
                        column: x => x.WareHouseDest,
                        principalTable: "IAW_Warehouse",
                        principalColumn: "WarehouseId");
                    table.ForeignKey(
                        name: "FK_IAW_InventoryTxn_IAW_Warehouse_WarehouseOri",
                        column: x => x.WarehouseOri,
                        principalTable: "IAW_Warehouse",
                        principalColumn: "WarehouseId");
                });

            migrationBuilder.CreateTable(
                name: "IAW_Stock",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NroProduct = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    QtyOnHand = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    QtyReserved = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ReorderPoint = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_Stock", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_IAW_Stock_IAW_Product_NroProduct",
                        column: x => x.NroProduct,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IAW_Stock_IAW_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "IAW_Warehouse",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IAW_InvDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnId = table.Column<int>(type: "integer", nullable: false),
                    NroProduct = table.Column<int>(type: "integer", nullable: false),
                    QtyProduct = table.Column<decimal>(type: "numeric(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_InvDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_IAW_InvDetail_IAW_InventoryTxn_TxnId",
                        column: x => x.TxnId,
                        principalTable: "IAW_InventoryTxn",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IAW_InvDetail_IAW_Product_NroProduct",
                        column: x => x.NroProduct,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IAW_InvDetail_NroProduct",
                table: "IAW_InvDetail",
                column: "NroProduct");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_InvDetail_TxnId",
                table: "IAW_InvDetail",
                column: "TxnId");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_InventoryTxn_WareHouseDest",
                table: "IAW_InventoryTxn",
                column: "WareHouseDest");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_InventoryTxn_WarehouseOri",
                table: "IAW_InventoryTxn",
                column: "WarehouseOri");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_Stock_NroProduct",
                table: "IAW_Stock",
                column: "NroProduct");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_Stock_WarehouseId",
                table: "IAW_Stock",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IAW_InvDetail");

            migrationBuilder.DropTable(
                name: "IAW_Stock");

            migrationBuilder.DropTable(
                name: "IAW_InventoryTxn");
        }
    }
}
