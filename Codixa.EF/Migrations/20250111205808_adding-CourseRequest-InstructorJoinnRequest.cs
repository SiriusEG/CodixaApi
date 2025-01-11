using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Codixa.EF.Migrations
{
    /// <inheritdoc />
    public partial class addingCourseRequestInstructorJoinnRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d61f69f-842d-4b7a-9308-7a35cd847b9b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a1699c8-f2fd-468e-89a9-7ee5d9c167ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e545899-0bbe-4224-9f03-7fed4b876565");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Enrollments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CourseRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_CourseRequests_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRequests_Instructors_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "Instructors",
                        principalColumn: "InstructorId");
                    table.ForeignKey(
                        name: "FK_CourseRequests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorJoinRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminRemarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorJoinRequests", x => x.RequestId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a3e2a59-f32d-4cad-904a-01ab1fa9e41a", null, "Admin", "ADMIN" },
                    { "6303743e-f26e-4b20-b9cb-d7878d519b8b", null, "Instructor", "INSTRUCTOR" },
                    { "77a7f207-6cb5-4a7c-8fbc-e98056e86e9c", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseRequests_CourseId",
                table: "CourseRequests",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRequests_ReviewedBy",
                table: "CourseRequests",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRequests_StudentId",
                table: "CourseRequests",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseRequests");

            migrationBuilder.DropTable(
                name: "InstructorJoinRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a3e2a59-f32d-4cad-904a-01ab1fa9e41a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6303743e-f26e-4b20-b9cb-d7878d519b8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77a7f207-6cb5-4a7c-8fbc-e98056e86e9c");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Enrollments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d61f69f-842d-4b7a-9308-7a35cd847b9b", null, "Admin", "ADMIN" },
                    { "9a1699c8-f2fd-468e-89a9-7ee5d9c167ad", null, "Student", "STUDENT" },
                    { "9e545899-0bbe-4224-9f03-7fed4b876565", null, "Instructor", "INSTRUCTOR" }
                });
        }
    }
}
