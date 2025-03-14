using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class editsectioncourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsOpened",
                table: "SectionProgress");

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessResult",
                table: "SectionTests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SectionType",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationTime",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "SuccessResult",
                table: "SectionTests");

            migrationBuilder.DropColumn(
                name: "SectionType",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DurationTime",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpened",
                table: "SectionProgress",
                type: "bit",
                nullable: false,
                defaultValue: false);

        }
    }
}
