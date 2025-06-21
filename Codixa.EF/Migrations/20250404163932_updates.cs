using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SectionProgress_SectionId",
                table: "SectionProgress");

   

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "SectionProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "LessonProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonProgress_Sections_SectionId",
                table: "LessonProgress",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionProgress_Courses_CourseId",
                table: "SectionProgress",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonProgress_Sections_SectionId",
                table: "LessonProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionProgress_Courses_CourseId",
                table: "SectionProgress");





            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "SectionProgress");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "LessonProgress");



            migrationBuilder.CreateIndex(
                name: "IX_SectionProgress_SectionId",
                table: "SectionProgress",
                column: "SectionId");
        }
    }
}
