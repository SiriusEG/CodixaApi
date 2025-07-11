using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    public partial class addingattemps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add AttemptId as nullable initially
            migrationBuilder.AddColumn<int>(
                name: "AttemptId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            // Step 2: Add MaxAttempts column to SectionTests
            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                table: "SectionTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Step 3: Create StudentTestAttempts table
            migrationBuilder.CreateTable(
                name: "StudentTestAttempts",
                columns: table => new
                {
                    AttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SectionTestId = table.Column<int>(type: "int", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    AttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestAttempts", x => x.AttemptId);
                    table.ForeignKey(
                        name: "FK_StudentTestAttempts_SectionTests_SectionTestId",
                        column: x => x.SectionTestId,
                        principalTable: "SectionTests",
                        principalColumn: "SectionTestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTestAttempts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAttempts_SectionTestId",
                table: "StudentTestAttempts",
                column: "SectionTestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAttempts_StudentId",
                table: "StudentTestAttempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AttemptId",
                table: "UserAnswers",
                column: "AttemptId");

            // Step 4: Backfill data for existing UserAnswers
            migrationBuilder.Sql(@"
                -- Insert missing attempts
                INSERT INTO StudentTestAttempts (StudentId, SectionTestId, AttemptNumber, AttemptDate)
                SELECT DISTINCT
                    UA.StudentId,
                    Q.SectionTestId,
                    1,
                    GETDATE()
                FROM UserAnswers UA
                JOIN Questions Q ON UA.QuestionId = Q.QuestionId
                WHERE NOT EXISTS (
                    SELECT 1 FROM StudentTestAttempts STA
                    WHERE STA.StudentId = UA.StudentId AND STA.SectionTestId = Q.SectionTestId
                );

                -- Update existing UserAnswers with valid AttemptId
                UPDATE UA
                SET AttemptId = STA.AttemptId
                FROM UserAnswers UA
                JOIN Questions Q ON UA.QuestionId = Q.QuestionId
                JOIN StudentTestAttempts STA
                    ON STA.StudentId = UA.StudentId AND STA.SectionTestId = Q.SectionTestId
                WHERE UA.AttemptId IS NULL;
            ");

            // Step 5: Make AttemptId non-nullable after data is valid
            migrationBuilder.AlterColumn<int>(
                name: "AttemptId",
                table: "UserAnswers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Step 6: Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_StudentTestAttempts_AttemptId",
                table: "UserAnswers",
                column: "AttemptId",
                principalTable: "StudentTestAttempts",
                principalColumn: "AttemptId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_StudentTestAttempts_AttemptId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "StudentTestAttempts");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_AttemptId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "AttemptId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                table: "SectionTests");
        }
    }
}
