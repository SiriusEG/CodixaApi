using Codixa.Core.Models.sharedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codixa.EF.Configurations
{
    public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
    {
        public void Configure(EntityTypeBuilder<Certification> builder)
        {
          builder.HasOne(c => c.Student)
            .WithMany(s => s.Certifications)
            .HasForeignKey(c => c.StudentId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
