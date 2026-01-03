using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sales1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AMS_Customer",
                columns: table => new
                {
                    NroPerson = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMS_Customer", x => x.NroPerson);
                    table.ForeignKey(
                        name: "FK_AMS_Customer_AMS_Person_NroPerson",
                        column: x => x.NroPerson,
                        principalTable: "AMS_Person",
                        principalColumn: "NroPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOM_SalesTxn",
                columns: table => new
                {
                    TxnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SOM_SalesTxn", x => x.TxnId);
                    table.ForeignKey(
                        name: "FK_SOM_SalesTxn_AMS_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AMS_Customer",
                        principalColumn: "NroPerson",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOM_SalesTxn_IAW_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "IAW_Warehouse",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOM_SalesDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SalesId = table.Column<int>(type: "integer", nullable: false),
                    NroProduct = table.Column<int>(type: "integer", nullable: false),
                    PresentCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    QtyPresent = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    QtyProduct = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOM_SalesDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_SOM_SalesDetail_IAW_Product_NroProduct",
                        column: x => x.NroProduct,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOM_SalesDetail_SOM_SalesTxn_SalesId",
                        column: x => x.SalesId,
                        principalTable: "SOM_SalesTxn",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOM_SalesDetail_NroProduct",
                table: "SOM_SalesDetail",
                column: "NroProduct");

            migrationBuilder.CreateIndex(
                name: "IX_SOM_SalesDetail_SalesId",
                table: "SOM_SalesDetail",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_SOM_SalesTxn_CustomerId",
                table: "SOM_SalesTxn",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SOM_SalesTxn_WarehouseId",
                table: "SOM_SalesTxn",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOM_SalesDetail");

            migrationBuilder.DropTable(
                name: "SOM_SalesTxn");

            migrationBuilder.DropTable(
                name: "AMS_Customer");
        }
    }
}
