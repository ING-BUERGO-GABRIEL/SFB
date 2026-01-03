using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class person1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstLastName",
                table: "AMS_Person");

            migrationBuilder.RenameColumn(
                name: "SecondLastName",
                table: "AMS_Person",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AMS_Person",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AMS_Person",
                newName: "SecondLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AMS_Person",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FirstLastName",
                table: "AMS_Person",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
