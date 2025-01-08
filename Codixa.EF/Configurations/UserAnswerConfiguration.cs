using Codixa.Core.Models.SectionsTestsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codixa.EF.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {


            builder.HasOne(ua => ua.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(ua => ua.Student)
                .WithMany(s => s.UserAnswers)
                .HasForeignKey(ua => ua.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
