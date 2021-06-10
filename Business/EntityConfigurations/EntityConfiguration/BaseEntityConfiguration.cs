using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business
{
    public class BaseEntityConfiguration<TDomain> : IEntityTypeConfiguration<TDomain> where TDomain : BaseDomain
    {
        public virtual void Configure(EntityTypeBuilder<TDomain> builder)
        {
            builder.HasIndex(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Active).IsRequired(true).HasDefaultValue(true);
            builder.Property(e => e.Created).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
            builder.Property(e => e.Updated).HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
            builder.Property(e => e.Visible).IsRequired(true).HasDefaultValue(true);

            builder.HasQueryFilter(e => e.Visible == true);
        }
    }
}
