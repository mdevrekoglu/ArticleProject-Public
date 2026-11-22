using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserImageAddedAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ResearcherId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<long>(
                name: "MajorDisciplineId",
                table: "Researchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", maxLength: 200000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Researchers_MajorDisciplineId",
                table: "Researchers",
                column: "MajorDisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedBy",
                table: "Notifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResearcherId",
                table: "AspNetUsers",
                column: "ResearcherId",
                unique: true,
                filter: "[ResearcherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserImages_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "UserImages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_CreatedBy",
                table: "Notifications",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Researchers_MajorDisciplines_MajorDisciplineId",
                table: "Researchers",
                column: "MajorDisciplineId",
                principalTable: "MajorDisciplines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserImages_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_CreatedBy",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Researchers_MajorDisciplines_MajorDisciplineId",
                table: "Researchers");

            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_Researchers_MajorDisciplineId",
                table: "Researchers");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CreatedBy",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ResearcherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MajorDisciplineId",
                table: "Researchers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "Lastname");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResearcherId",
                table: "AspNetUsers",
                column: "ResearcherId");
        }
    }
}
