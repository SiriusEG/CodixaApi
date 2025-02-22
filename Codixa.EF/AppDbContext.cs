using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using Codixa.EF.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
        public DbSet<LessonProgress> LessonProgress { get; set; }
        public DbSet<CourseProgress> CourseProgress { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ChoicesQuestion> ChoicesQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SectionTest> SectionTests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<InstructorJoinRequest> InstructorJoinRequests { get; set; }
        public DbSet<CourseRequest> CourseRequests { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
         
            var assembly = Assembly.Load("Codixa.Core");

       
            var keylessEntities = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IKeylessEntity).IsAssignableFrom(t));

      
            foreach (var entityType in keylessEntities)
            {
                var configurationType = typeof(KeylessEntityConfiguration<>).MakeGenericType(entityType);
                var configuration = Activator.CreateInstance(configurationType);
                builder.ApplyConfiguration((dynamic)configuration);
            }
        }
        
    }
}
