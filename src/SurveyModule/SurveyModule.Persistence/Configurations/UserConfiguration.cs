using CoreModule.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyModule.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(x => x.MobileNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.EmailAddress)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PasswordSalt)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.UserStatusId)
                .HasDefaultValue(((int)UserStatusType.WaitingForApproval));

            builder.Property(x => x.CreationDate)
                .HasColumnType("date");

            builder.Property(x => x.ModifiedDate)
                .HasColumnType("date");
        }
    }
}

