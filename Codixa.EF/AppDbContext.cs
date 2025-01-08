using Codixa.Core.Models;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.EF
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {


        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<courseFeedback> courseFeedbacks { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ChoicesQuestion> ChoicesQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SectionTest> SectionTests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
    }
}
