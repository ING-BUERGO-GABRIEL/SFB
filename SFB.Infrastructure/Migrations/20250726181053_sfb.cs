using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sfb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AMS_Person",
                columns: table => new
                {
                    NroPerson = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FirstLastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    SecondLastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    DateBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMS_Person", x => x.NroPerson);
                });

            migrationBuilder.CreateTable(
                name: "AMS_TypePerson",
                columns: table => new
                {
                    NroPerson = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMS_TypePerson", x => new { x.NroPerson, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "CMS_Module",
                columns: table => new
                {
                    CodModule = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Module", x => x.CodModule);
                });

            migrationBuilder.CreateTable(
                name: "CMS_OptionMenu",
                columns: table => new
                {
                    CodOption = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CodGruOption = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Route = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "IAW_Product",
                columns: table => new
                {
                    NroProduct = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    IsPurchases = table.Column<bool>(type: "boolean", nullable: false),
                    IsSales = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAW_Product", x => x.NroProduct);
                });

            migrationBuilder.CreateTable(
                name: "AMS_User",
                columns: table => new
                {
                    NameUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    UserType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    NroPerson = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMS_User", x => x.NameUser);
                    table.ForeignKey(
                        name: "FK_AMS_User_AMS_Person_NroPerson",
                        column: x => x.NroPerson,
                        principalTable: "AMS_Person",
                        principalColumn: "NroPerson",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Group",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CodModule = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AMS_UserGroup",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "integer", nullable: false),
                    NameUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "CMS_GroupOption",
                columns: table => new
                {
                    NroGroup = table.Column<int>(type: "integer", nullable: false),
                    CodOption = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_AMS_User_NroPerson",
                table: "AMS_User",
                column: "NroPerson");

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
                name: "AMS_TypePerson");

            migrationBuilder.DropTable(
                name: "AMS_UserGroup");

            migrationBuilder.DropTable(
                name: "CMS_GroupOption");

            migrationBuilder.DropTable(
                name: "IAW_Product");

            migrationBuilder.DropTable(
                name: "AMS_User");

            migrationBuilder.DropTable(
                name: "CMS_Group");

            migrationBuilder.DropTable(
                name: "CMS_OptionMenu");

            migrationBuilder.DropTable(
                name: "AMS_Person");

            migrationBuilder.DropTable(
                name: "CMS_Module");
        }
    }
}
