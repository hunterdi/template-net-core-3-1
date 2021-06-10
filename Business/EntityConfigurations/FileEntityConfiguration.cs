using Business;
using Business.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class FileEntityConfiguration: BaseEntityConfiguration<File>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<File> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AccountID).IsRequired();
            builder.Property(e => e.ContentType).IsRequired();
            builder.Property(e => e.Extension).IsRequired();
            builder.Property(e => e.FileBytes).IsRequired();
            builder.Property(e => e.FileName).IsRequired();
            builder.Property(e => e.FilePath);
            builder.Property(e => e.HashName).IsRequired();
            builder.Property(e => e.Size).IsRequired();
        }
    }
}
