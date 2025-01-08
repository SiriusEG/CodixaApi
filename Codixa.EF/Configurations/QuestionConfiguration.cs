using Codixa.Core.Models.SectionsTestsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codixa.EF.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            
            builder.HasOne(q => q.SectionTest)
            .WithMany(st => st.Questions)
            .HasForeignKey(q => q.SectionTestId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
