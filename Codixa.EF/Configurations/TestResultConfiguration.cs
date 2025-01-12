using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.sharedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codixa.EF.Configurations
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {



            builder.HasOne(tr => tr.Certification)
                .WithOne(c => c.TestResult)
                .HasForeignKey<Certification>(c => c.TestResultId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(tr => tr.Student)
                .WithMany(s => s.TestResults)
                .HasForeignKey(tr => tr.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
