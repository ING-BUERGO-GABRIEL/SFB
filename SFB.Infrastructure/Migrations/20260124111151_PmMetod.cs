using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PmMetod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethodCode",
                table: "TRM_TreasuryDetail",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.CreateTable(
                name: "SOM_PaymentMethod",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TxnId = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethodCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PaymentRef = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOM_PaymentMethod", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_SOM_PaymentMethod_SOM_SalesTxn_TxnId",
                        column: x => x.TxnId,
                        principalTable: "SOM_SalesTxn",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOM_PaymentMethod_TxnId",
                table: "SOM_PaymentMethod",
                column: "TxnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOM_PaymentMethod");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethodCode",
                table: "TRM_TreasuryDetail",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4);
        }
    }
}
