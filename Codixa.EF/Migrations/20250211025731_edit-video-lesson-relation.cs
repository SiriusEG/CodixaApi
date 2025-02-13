using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class editvideolessonrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
  
            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "Lessons",
                type: "nvarchar(450)",
                nullable: true);

    

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Files_VideoId",
                table: "Lessons",
                column: "VideoId",
                principalTable: "Files",
                principalColumn: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Files_VideoId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons");


            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

   
        }
    }
}
