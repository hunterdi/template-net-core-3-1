using Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class TaskListEntityConfiguration : BaseEntityConfiguration<TaskList>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<TaskList> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();
            
            builder.HasMany(e => e.Tasks)
                .WithOne(e => e.TaskLists)
                .HasForeignKey(e => e.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
