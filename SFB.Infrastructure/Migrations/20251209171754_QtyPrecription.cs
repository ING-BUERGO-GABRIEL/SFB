using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QtyPrecription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PresentCode",
                table: "IAW_Product",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PresentCode",
                table: "IAW_InvDetail",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "QtyPresent",
                table: "IAW_InvDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "IAW_Presentation",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_Presentation", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "IAW_ProductPresent",
                columns: table => new
                {
                    PresentCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    QtyProduct = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_ProductPresent", x => new { x.ProductId, x.PresentCode });
                    table.ForeignKey(
                        name: "FK_IAW_ProductPresent_IAW_Presentation_PresentCode",
                        column: x => x.PresentCode,
                        principalTable: "IAW_Presentation",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IAW_ProductPresent_IAW_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IAW_Product_PresentCode",
                table: "IAW_Product",
                column: "PresentCode");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_ProductPresent_PresentCode",
                table: "IAW_ProductPresent",
                column: "PresentCode");

            migrationBuilder.AddForeignKey(
                name: "FK_IAW_Product_IAW_Presentation_PresentCode",
                table: "IAW_Product",
                column: "PresentCode",
                principalTable: "IAW_Presentation",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAW_Product_IAW_Presentation_PresentCode",
                table: "IAW_Product");

            migrationBuilder.DropTable(
                name: "IAW_ProductPresent");

            migrationBuilder.DropTable(
                name: "IAW_Presentation");

            migrationBuilder.DropIndex(
                name: "IX_IAW_Product_PresentCode",
                table: "IAW_Product");

            migrationBuilder.DropColumn(
                name: "PresentCode",
                table: "IAW_Product");

            migrationBuilder.DropColumn(
                name: "PresentCode",
                table: "IAW_InvDetail");

            migrationBuilder.DropColumn(
                name: "QtyPresent",
                table: "IAW_InvDetail");
        }
    }
}
