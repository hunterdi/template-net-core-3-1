using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TagTaskEntityConfiguration : BaseEntityConfiguration<TagTask>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<TagTask> builder)
        {
            base.Configure(builder);

            builder.HasIndex(e => new { e.TagId, e.TaskId }).IsUnique();

            builder.HasOne(e => e.Tag)
                .WithMany(e => e.TagsTasks)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Task)
                .WithMany(e => e.TagsTasks)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
