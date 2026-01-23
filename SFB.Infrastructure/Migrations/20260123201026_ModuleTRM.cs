using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuleTRM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRM_CashBox",
                columns: table => new
                {
                    CashBoxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Type = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRM_CashBox", x => x.CashBoxId);
                });

            migrationBuilder.CreateTable(
                name: "TRM_TreasuryTxn",
                columns: table => new
                {
                    TxnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnOrigin = table.Column<int>(type: "integer", nullable: true),
                    ModOrigin = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Type = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    StatusCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TxnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CashBoxOriginId = table.Column<int>(type: "integer", nullable: true),
                    CashBoxDestId = table.Column<int>(type: "integer", nullable: true),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CashierId = table.Column<int>(type: "integer", nullable: true),
                    Reference = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRM_TreasuryTxn", x => x.TxnId);
                    table.ForeignKey(
                        name: "FK_TRM_TreasuryTxn_TRM_CashBox_CashBoxDestId",
                        column: x => x.CashBoxDestId,
                        principalTable: "TRM_CashBox",
                        principalColumn: "CashBoxId");
                    table.ForeignKey(
                        name: "FK_TRM_TreasuryTxn_TRM_CashBox_CashBoxOriginId",
                        column: x => x.CashBoxOriginId,
                        principalTable: "TRM_CashBox",
                        principalColumn: "CashBoxId");
                });

            migrationBuilder.CreateTable(
                name: "TRM_TreasuryDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnId = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethodCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PaymentRef = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRM_TreasuryDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_TRM_TreasuryDetail_TRM_TreasuryTxn_TxnId",
                        column: x => x.TxnId,
                        principalTable: "TRM_TreasuryTxn",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRM_TreasuryDetail_TxnId",
                table: "TRM_TreasuryDetail",
                column: "TxnId");

            migrationBuilder.CreateIndex(
                name: "IX_TRM_TreasuryTxn_CashBoxDestId",
                table: "TRM_TreasuryTxn",
                column: "CashBoxDestId");

            migrationBuilder.CreateIndex(
                name: "IX_TRM_TreasuryTxn_CashBoxOriginId",
                table: "TRM_TreasuryTxn",
                column: "CashBoxOriginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRM_TreasuryDetail");

            migrationBuilder.DropTable(
                name: "TRM_TreasuryTxn");

            migrationBuilder.DropTable(
                name: "TRM_CashBox");
        }
    }
}
