using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class formExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TAM_FormTeacher",
                columns: table => new
                {
                    NroForm = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<int>(type: "integer", nullable: false),
                    ProposalTitle = table.Column<string>(type: "text", nullable: false),
                    CodScope = table.Column<string>(type: "text", nullable: false),
                    CodModality = table.Column<string>(type: "text", nullable: false),
                    CodArea = table.Column<int>(type: "integer", nullable: false),
                    CodSchoolYear = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    NameTeache = table.Column<string>(type: "text", nullable: false),
                    NameSchool = table.Column<string>(type: "text", nullable: false),
                    CodDepartment = table.Column<string>(type: "text", nullable: false),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    CodStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAM_FormTeacher", x => x.NroForm);
                });

            migrationBuilder.CreateTable(
                name: "TAM_TeacherTask",
                columns: table => new
                {
                    NroTask = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameContact = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NroFromTeacher = table.Column<int>(type: "integer", nullable: false),
                    PriceTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    NroPersonReturn = table.Column<int>(type: "integer", nullable: false),
                    NroPersonAssigned = table.Column<int>(type: "integer", nullable: true),
                    CodStatus = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    UrlDocument = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserReg = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateReg = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserUpd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateUpd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAM_TeacherTask", x => x.NroTask);
                    table.ForeignKey(
                        name: "FK_TAM_TeacherTask_AMS_Person_NroPersonAssigned",
                        column: x => x.NroPersonAssigned,
                        principalTable: "AMS_Person",
                        principalColumn: "NroPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TAM_TeacherTask_AMS_Person_NroPersonReturn",
                        column: x => x.NroPersonReturn,
                        principalTable: "AMS_Person",
                        principalColumn: "NroPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TAM_TeacherTask_TAM_FormTeacher_NroFromTeacher",
                        column: x => x.NroFromTeacher,
                        principalTable: "TAM_FormTeacher",
                        principalColumn: "NroForm",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TAM_TeacherTask_NroFromTeacher",
                table: "TAM_TeacherTask",
                column: "NroFromTeacher");

            migrationBuilder.CreateIndex(
                name: "IX_TAM_TeacherTask_NroPersonAssigned",
                table: "TAM_TeacherTask",
                column: "NroPersonAssigned");

            migrationBuilder.CreateIndex(
                name: "IX_TAM_TeacherTask_NroPersonReturn",
                table: "TAM_TeacherTask",
                column: "NroPersonReturn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAM_TeacherTask");

            migrationBuilder.DropTable(
                name: "TAM_FormTeacher");
        }
    }
}
