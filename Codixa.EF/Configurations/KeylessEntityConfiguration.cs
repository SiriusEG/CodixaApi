using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Codixa.EF.Configurations
{
    public class KeylessEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
     where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // تطبيق التكوين على الكيان
            builder.HasNoKey().ToView(null);
        }
    }
}
