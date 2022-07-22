using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Domain
{
    public class User : Auditable
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

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
        }
    }

    public static class UserExtensions
    {
        public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Email, user.Password);
    }

    public class UserDto
    {
        public long Id { get; }
        public string Email { get; }
        public string Password { get; }

        public UserDto(long id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }

}
