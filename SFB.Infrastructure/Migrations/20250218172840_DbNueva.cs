using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbNueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Group",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodModule = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Group", x => x.NroGroup);
                    table.ForeignKey(
                        name: "FK_CMS_Group_CMS_Module_CodModule",
                        column: x => x.CodModule,
                        principalTable: "CMS_Module",
                        principalColumn: "CodModule",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CMS_OptionMenu",
                columns: table => new
                {
                    CodOption = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodGruOption = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Route = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_OptionMenu", x => x.CodOption);
                    table.ForeignKey(
                        name: "FK_CMS_OptionMenu_CMS_OptionMenu_CodGruOption",
                        column: x => x.CodGruOption,
                        principalTable: "CMS_OptionMenu",
                        principalColumn: "CodOption",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AMS_UserGroup",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "int", nullable: false),
                    NameUser = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMS_UserGroup", x => new { x.NameUser, x.NroGroup });
                    table.ForeignKey(
                        name: "FK_AMS_UserGroup_AMS_User_NameUser",
                        column: x => x.NameUser,
                        principalTable: "AMS_User",
                        principalColumn: "NameUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AMS_UserGroup_CMS_Group_NroGroup",
                        column: x => x.NroGroup,
                        principalTable: "CMS_Group",
                        principalColumn: "NroGroup",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CMS_GroupOption",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "int", nullable: false),
                    CodOption = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_GroupOption", x => new { x.NroGroup, x.CodOption });
                    table.ForeignKey(
                        name: "FK_CMS_GroupOption_CMS_Group_NroGroup",
                        column: x => x.NroGroup,
                        principalTable: "CMS_Group",
                        principalColumn: "NroGroup",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_GroupOption_CMS_OptionMenu_CodOption",
                        column: x => x.CodOption,
                        principalTable: "CMS_OptionMenu",
                        principalColumn: "CodOption",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AMS_UserGroup_NroGroup",
                table: "AMS_UserGroup",
                column: "NroGroup");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Group_CodModule",
                table: "CMS_Group",
                column: "CodModule");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_GroupOption_CodOption",
                table: "CMS_GroupOption",
                column: "CodOption");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_OptionMenu_CodGruOption",
                table: "CMS_OptionMenu",
                column: "CodGruOption");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AMS_UserGroup");

            migrationBuilder.DropTable(
                name: "CMS_GroupOption");

            migrationBuilder.DropTable(
                name: "CMS_Group");

            migrationBuilder.DropTable(
                name: "CMS_OptionMenu");
        }
    }
}
