using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codixa.EF.Migrations
{
    public partial class addingattempidtotestresult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add column as nullable
            migrationBuilder.AddColumn<int>(
                name: "AttemptId",
                table: "TestResults",
                type: "int",
                nullable: true);

            // Step 2: Backfill with best-matching attempt (first attempt per Student+SectionTest)
            migrationBuilder.Sql(@"
                UPDATE TR
                SET AttemptId = STA.AttemptId
                FROM TestResults TR
                JOIN StudentTestAttempts STA
                    ON TR.StudentId = STA.StudentId
                   AND TR.SectionTestId = STA.SectionTestId
                WHERE TR.AttemptId IS NULL;
            ");

            // Step 3: Make column NOT NULL after backfill
            migrationBuilder.AlterColumn<int>(
                name: "AttemptId",
                table: "TestResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Step 4: Add foreign key constraint
            migrationBuilder.CreateIndex(
                name: "IX_TestResults_AttemptId",
                table: "TestResults",
                column: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_StudentTestAttempts_AttemptId",
                table: "TestResults",
                column: "AttemptId",
                principalTable: "StudentTestAttempts",
                principalColumn: "AttemptId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_StudentTestAttempts_AttemptId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_AttemptId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "AttemptId",
                table: "TestResults");
        }
    }
}
