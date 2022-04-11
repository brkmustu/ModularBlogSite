using CoreModule.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreModule.Configurations;

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
            .IsRequired(false)
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
            .HasDefaultValue(false);

        builder.Property(x => x.UserStatusId)
            .HasDefaultValue(((int)UserStatusType.WaitingForApproval));

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(x => x.LastModifiedDate)
            .IsRequired(false)
            .HasColumnType("date");

        builder.Property(x => x.LastModifiedBy)
            .IsRequired(false);

        builder.Property(x => x.RoleIds)
            .IsRequired(false);
    }
}

