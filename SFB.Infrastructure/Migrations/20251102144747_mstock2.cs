using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mstock2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IAW_Stock",
                table: "IAW_Stock");

            migrationBuilder.DropIndex(
                name: "IX_IAW_Stock_NroProduct",
                table: "IAW_Stock");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "IAW_Stock");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IAW_Stock",
                table: "IAW_Stock",
                columns: new[] { "NroProduct", "WarehouseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IAW_Stock",
                table: "IAW_Stock");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "IAW_Stock",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IAW_Stock",
                table: "IAW_Stock",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_IAW_Stock_NroProduct",
                table: "IAW_Stock",
                column: "NroProduct");
        }
    }
}
