using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvTxnType2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "IAW_InventoryTxn",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "IAW_InventoryTxn");
        }
    }
}
