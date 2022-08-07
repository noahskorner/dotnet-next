using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(DataConfiguration.SHORT_STRING_LENGTH);

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.Property(x => x.Password)
                  .IsRequired()
                  .HasMaxLength(DataConfiguration.SHORT_STRING_LENGTH);

            entity.Property(x => x.EmailVerificationToken)
                  .IsRequired(false)
                  .HasMaxLength(DataConfiguration.SHORT_STRING_LENGTH);

            entity.Property(x => x.IsEmailVerified)
                  .HasDefaultValue(false);

            entity.HasIndex(x => x.CreatedAt);
        }
    }
}
