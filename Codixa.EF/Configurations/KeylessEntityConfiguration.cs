using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
