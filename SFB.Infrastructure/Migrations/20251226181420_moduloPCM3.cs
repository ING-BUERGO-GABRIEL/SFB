using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moduloPCM3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCM_PurchaseTxn_ESupplier_SupplierId",
                table: "PCM_PurchaseTxn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ESupplier",
                table: "ESupplier");

            migrationBuilder.RenameTable(
                name: "ESupplier",
                newName: "PCM_Supplier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PCM_Supplier",
                table: "PCM_Supplier",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCM_PurchaseTxn_PCM_Supplier_SupplierId",
                table: "PCM_PurchaseTxn",
                column: "SupplierId",
                principalTable: "PCM_Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCM_PurchaseTxn_PCM_Supplier_SupplierId",
                table: "PCM_PurchaseTxn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PCM_Supplier",
                table: "PCM_Supplier");

            migrationBuilder.RenameTable(
                name: "PCM_Supplier",
                newName: "ESupplier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ESupplier",
                table: "ESupplier",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCM_PurchaseTxn_ESupplier_SupplierId",
                table: "PCM_PurchaseTxn",
                column: "SupplierId",
                principalTable: "ESupplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
