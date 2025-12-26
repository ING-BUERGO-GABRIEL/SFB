using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moduloPCM2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NroProduct",
                table: "PCM_PurDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PresentCode",
                table: "PCM_PurDetail",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "QtyPresent",
                table: "PCM_PurDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyProduct",
                table: "PCM_PurDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "PCM_PurDetail",
                type: "numeric(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitCost",
                table: "PCM_PurDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PCM_PurDetail_NroProduct",
                table: "PCM_PurDetail",
                column: "NroProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_PCM_PurDetail_IAW_Product_NroProduct",
                table: "PCM_PurDetail",
                column: "NroProduct",
                principalTable: "IAW_Product",
                principalColumn: "NroProduct",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCM_PurDetail_IAW_Product_NroProduct",
                table: "PCM_PurDetail");

            migrationBuilder.DropIndex(
                name: "IX_PCM_PurDetail_NroProduct",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "NroProduct",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "PresentCode",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "QtyPresent",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "QtyProduct",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "PCM_PurDetail");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "PCM_PurDetail");
        }
    }
}
