using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SalesSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOM_SalesSettings",
                columns: table => new
                {
                    SettingId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BranchCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SettingsGroup = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ConfigCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NameCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DesCode = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ValueCode = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ConfigType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OriginConfig = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Order = table.Column<short>(type: "smallint", nullable: false),
                    StatusCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOM_SalesSettings", x => x.SettingId);
                    table.ForeignKey(
                        name: "FK_SOM_SalesSettings_SOM_SalesSettings_SettingsGroup",
                        column: x => x.SettingsGroup,
                        principalTable: "SOM_SalesSettings",
                        principalColumn: "SettingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOM_SalesSettings_SettingsGroup",
                table: "SOM_SalesSettings",
                column: "SettingsGroup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOM_SalesSettings");
        }
    }
}
