﻿// <auto-generated />
using System;
using Codxia.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Codixa.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250111205808_adding-CourseRequest-InstructorJoinnRequest")]
    partial class addingCourseRequestInstructorJoinnRequest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Codixa.Core.Models.Certification", b =>
                {
                    b.Property<int>("CertificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CertificationId"));

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TestResultId")
                        .HasColumnType("int");

                    b.HasKey("CertificationId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TestResultId")
                        .IsUnique();

                    b.ToTable("Certifications");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommentId");

                    b.HasIndex("LessonId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CourseDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstructorId")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.CourseRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequestStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReviewedBy")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("RequestId");

                    b.HasIndex("CourseId");

                    b.HasIndex("ReviewedBy");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseRequests");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Lesson", b =>
                {
                    b.Property<int>("LessonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LessonId"));

                    b.Property<bool>("IsVideo")
                        .HasColumnType("bit");

                    b.Property<string>("LessonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonOrder")
                        .HasColumnType("int");

                    b.Property<string>("LessonText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("VideoLink")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LessonId");

                    b.HasIndex("SectionId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Section", b =>
                {
                    b.Property<int>("SectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SectionId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("SectionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SectionOrder")
                        .HasColumnType("int");

                    b.HasKey("SectionId");

                    b.HasIndex("CourseId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.courseFeedback", b =>
                {
                    b.Property<int>("FeedBackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedBackId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("FeedBackId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("courseFeedbacks");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.ChoicesQuestion", b =>
                {
                    b.Property<int>("ChoicesQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChoicesQuestionId"));

                    b.Property<string>("ChoicesQuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsTrue")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("ChoicesQuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ChoicesQuestions");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<int>("SectionTestId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.HasIndex("SectionTestId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.SectionTest", b =>
                {
                    b.Property<int>("SectionTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SectionTestId"));

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.HasKey("SectionTestId");

                    b.HasIndex("SectionId")
                        .IsUnique();

                    b.ToTable("SectionTests");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.TestResult", b =>
                {
                    b.Property<int>("TestResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TestResultId"));

                    b.Property<bool>("IsPassed")
                        .HasColumnType("bit");

                    b.Property<decimal>("Result")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SectionTestId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("TestResultId");

                    b.HasIndex("SectionTestId");

                    b.HasIndex("StudentId");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.UserAnswer", b =>
                {
                    b.Property<int>("UserAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserAnswerId"));

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("SelectedChoicesQuestionId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("UserAnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SelectedChoicesQuestionId");

                    b.HasIndex("StudentId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Instructor", b =>
                {
                    b.Property<int>("InstructorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstructorId"));

                    b.Property<string>("InstructorFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("InstructorId");

                    b.HasIndex("UserId");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.InstructorJoinRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<string>("AdminRemarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("RequestId");

                    b.ToTable("InstructorJoinRequests");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("StudentFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StudentId");

                    b.HasIndex("UserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1a3e2a59-f32d-4cad-904a-01ab1fa9e41a",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "6303743e-f26e-4b20-b9cb-d7878d519b8b",
                            Name = "Instructor",
                            NormalizedName = "INSTRUCTOR"
                        },
                        new
                        {
                            Id = "77a7f207-6cb5-4a7c-8fbc-e98056e86e9c",
                            Name = "Student",
                            NormalizedName = "STUDENT"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Codixa.Core.Models.Certification", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("Certifications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.TestResult", "TestResult")
                        .WithOne("Certification")
                        .HasForeignKey("Codixa.Core.Models.Certification", "TestResultId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("TestResult");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Comment", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Lesson", "Lesson")
                        .WithMany("Comments")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Course", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Category", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Instructor", "Instructor")
                        .WithMany("Courses")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.CourseRequest", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Course", "Course")
                        .WithMany("CourseRequests")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Instructor", "ReviewedByInstructor")
                        .WithMany("ReviewedRequests")
                        .HasForeignKey("ReviewedBy");

                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("CourseRequests")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("ReviewedByInstructor");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Enrollment", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Lesson", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Section", "Section")
                        .WithMany("Lessons")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Section", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Course", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.courseFeedback", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Course", "Course")
                        .WithMany("courseFeedbacks")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("courseFeedbacks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.ChoicesQuestion", b =>
                {
                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.Question", "Question")
                        .WithMany("ChoicesQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.Question", b =>
                {
                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.SectionTest", "SectionTest")
                        .WithMany("Questions")
                        .HasForeignKey("SectionTestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SectionTest");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.SectionTest", b =>
                {
                    b.HasOne("Codixa.Core.Models.CourseModels.Section", "Section")
                        .WithOne("SectionTest")
                        .HasForeignKey("Codixa.Core.Models.SectionsTestsModels.SectionTest", "SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.TestResult", b =>
                {
                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.SectionTest", "SectionTest")
                        .WithMany()
                        .HasForeignKey("SectionTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("TestResults")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SectionTest");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.UserAnswer", b =>
                {
                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.Question", "Question")
                        .WithMany("UserAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.SectionsTestsModels.ChoicesQuestion", "ChoicesQuestion")
                        .WithMany()
                        .HasForeignKey("SelectedChoicesQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.Student", "Student")
                        .WithMany("UserAnswers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChoicesQuestion");

                    b.Navigation("Question");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Instructor", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Student", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Codixa.Core.Models.UserModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Category", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Course", b =>
                {
                    b.Navigation("CourseRequests");

                    b.Navigation("Enrollments");

                    b.Navigation("Sections");

                    b.Navigation("courseFeedbacks");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Lesson", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Codixa.Core.Models.CourseModels.Section", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("SectionTest")
                        .IsRequired();
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.Question", b =>
                {
                    b.Navigation("ChoicesQuestions");

                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.SectionTest", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Codixa.Core.Models.SectionsTestsModels.TestResult", b =>
                {
                    b.Navigation("Certification")
                        .IsRequired();
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Instructor", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("ReviewedRequests");
                });

            modelBuilder.Entity("Codixa.Core.Models.UserModels.Student", b =>
                {
                    b.Navigation("Certifications");

                    b.Navigation("CourseRequests");

                    b.Navigation("Enrollments");

                    b.Navigation("TestResults");

                    b.Navigation("UserAnswers");

                    b.Navigation("courseFeedbacks");
                });
#pragma warning restore 612, 618
        }
    }
}
