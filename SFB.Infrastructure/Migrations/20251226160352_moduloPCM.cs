using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moduloPCM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESupplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESupplier", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "PCM_PurDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCM_PurDetail", x => x.DetailId);
                });

            migrationBuilder.CreateTable(
                name: "PCM_PurchaseTxn",
                columns: table => new
                {
                    TxnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    StatusCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Type = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    Reference = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCM_PurchaseTxn", x => x.TxnId);
                    table.ForeignKey(
                        name: "FK_PCM_PurchaseTxn_ESupplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "ESupplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCM_PurchaseTxn_IAW_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "IAW_Warehouse",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PCM_PurchaseDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseId = table.Column<int>(type: "integer", nullable: false),
                    NroProduct = table.Column<int>(type: "integer", nullable: false),
                    PresentCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    QtyPresent = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    QtyProduct = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCM_PurchaseDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_PCM_PurchaseDetail_IAW_Product_NroProduct",
                        column: x => x.NroProduct,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCM_PurchaseDetail_PCM_PurchaseTxn_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "PCM_PurchaseTxn",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurchaseDetail_NroProduct",
                table: "PCM_PurchaseDetail",
                column: "NroProduct");

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurchaseDetail_PurchaseId",
                table: "PCM_PurchaseDetail",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurchaseTxn_SupplierId",
                table: "PCM_PurchaseTxn",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurchaseTxn_WarehouseId",
                table: "PCM_PurchaseTxn",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCM_PurchaseDetail");

            migrationBuilder.DropTable(
                name: "PCM_PurDetail");

            migrationBuilder.DropTable(
                name: "PCM_PurchaseTxn");

            migrationBuilder.DropTable(
                name: "ESupplier");
        }
    }
}
