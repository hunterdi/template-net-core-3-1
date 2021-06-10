using Business;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business
{
    public class TagEntityConfiguration : BaseEntityConfiguration<Tag>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();
        }
    }
}
