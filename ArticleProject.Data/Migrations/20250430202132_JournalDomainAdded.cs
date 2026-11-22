using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class JournalDomainAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quartile = table.Column<int>(type: "int", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    TotalJournals = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AbbreviationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ISSN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EISSN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    JIF = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    JIF5Years = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journals_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journals_AbbreviationName",
                table: "Journals",
                column: "AbbreviationName");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_CategoryId",
                table: "Journals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_EISSN",
                table: "Journals",
                column: "EISSN");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_EISSN_Year",
                table: "Journals",
                columns: new[] { "EISSN", "Year" },
                unique: true,
                filter: "[EISSN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_ISSN",
                table: "Journals",
                column: "ISSN");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_ISSN_Year",
                table: "Journals",
                columns: new[] { "ISSN", "Year" },
                unique: true,
                filter: "[ISSN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_Name",
                table: "Journals",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journals");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
