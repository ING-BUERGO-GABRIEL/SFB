using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moduloPCM4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCM_PurDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PCM_PurDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NroProduct = table.Column<int>(type: "integer", nullable: false),
                    PresentCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    QtyPresent = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    QtyProduct = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    TxnId = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCM_PurDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_PCM_PurDetail_IAW_Product_NroProduct",
                        column: x => x.NroProduct,
                        principalTable: "IAW_Product",
                        principalColumn: "NroProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurDetail_NroProduct",
                table: "PCM_PurDetail",
                column: "NroProduct");
        }
    }
}
