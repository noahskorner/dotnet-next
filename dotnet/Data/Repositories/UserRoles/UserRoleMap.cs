using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repositories.UserRoles
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> entity)
        {
            entity.HasOne(x => x.User)
                .WithMany(x => x.UserRoles);

            entity.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles);

            entity.HasIndex(x => new { x.UserId, x.RoleId })
                .IsUnique();
        }
    }
}
