using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
