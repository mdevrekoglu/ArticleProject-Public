using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AzureFileAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AzureFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    ReportType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AzureFiles_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AzureFiles_CreatedBy",
                table: "AzureFiles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AzureFiles_FileName",
                table: "AzureFiles",
                column: "FileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AzureFiles_GUID",
                table: "AzureFiles",
                column: "GUID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AzureFiles");
        }
    }
}
