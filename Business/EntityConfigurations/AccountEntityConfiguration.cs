using Business;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business
{
    public class AccountEntityConfiguration : BaseEntityConfiguration<Account>, IEntityMapping
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AcceptTerms);
            builder.Property(e => e.Email);
            builder.Property(e => e.FirstName);
            builder.Ignore(e => e.IsVerified);
            builder.Ignore(e => e.Password);
            builder.Property(e => e.LastName);
            builder.Property(e => e.PasswordHash);
            builder.Property(e => e.PasswordReset);
            builder.Property(e => e.ResetToken);
            builder.Property(e => e.ResetTokenExpires);
            builder.Property(e => e.Role);
            builder.Property(e => e.Title);
            builder.Property(e => e.VerificationToken);
            builder.Property(e => e.Verified);
        }
    }
}
