using Business;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business
{
    public class TaskEntityConfiguration : BaseEntityConfiguration<Tasks>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<Tasks> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.Priority).IsRequired();
            builder.Property(e => e.TaskListId).IsRequired();
        }
    }
}
