using Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repositories.UserRoles
{
    public class RoleMap : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> entity)
        {
            entity.Property(x => x.Name)
                .HasMaxLength(DataConfiguration.SHORT_STRING_LENGTH);
        }
    }
}
