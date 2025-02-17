using Codixa.Core.Models.CourseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codixa.EF.Configurations
{
    public class courseFeedbackConfiguration : IEntityTypeConfiguration<courseFeedback>
    {
        public void Configure(EntityTypeBuilder<courseFeedback> builder)
        {

            builder.HasOne(cf => cf.Course)
            .WithMany(c => c.courseFeedbacks)
            .HasForeignKey(cf => cf.CourseId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
