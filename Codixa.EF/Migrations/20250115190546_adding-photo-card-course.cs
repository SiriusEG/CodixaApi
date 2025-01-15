using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class addingphotocardcourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   

            migrationBuilder.AddColumn<string>(
                name: "CourseCardPhotoId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

  

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCardPhotoId",
                table: "Courses",
                column: "CourseCardPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Files_CourseCardPhotoId",
                table: "Courses",
                column: "CourseCardPhotoId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Files_CourseCardPhotoId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseCardPhotoId",
                table: "Courses");

          

            migrationBuilder.DropColumn(
                name: "CourseCardPhotoId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");

       
        }
    }
}
