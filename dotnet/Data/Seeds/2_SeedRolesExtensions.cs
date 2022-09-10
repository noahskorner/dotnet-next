using Data.Repositories.UserRoles;
using Domain.Constants;

namespace Data.Seeds
{
    public static class SeedRolesExtensions
    {
        public static void SeedRoles(this ApiContext context)
        {
            var roles = new List<RoleEntity>() { 
                new RoleEntity() { Name = Roles.SUPER_ADMIN },
                new RoleEntity() { Name = Roles.USER }
            };
            
            var existingRoles = context.Roles.ToList();
            var newRoles = roles.Except(existingRoles);

            context.AddRange(newRoles);
            context.SaveChanges();
        }
    }
}
