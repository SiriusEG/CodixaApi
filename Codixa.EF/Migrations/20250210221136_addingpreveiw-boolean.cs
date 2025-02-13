using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class addingpreveiwboolean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.AddColumn<bool>(
                name: "IsForpreview",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

     
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "IsForpreview",
                table: "Lessons");

 
        }
    }
}
