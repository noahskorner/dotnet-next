using Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities.Users
{
    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> entity)
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
