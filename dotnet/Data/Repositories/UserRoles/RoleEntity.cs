namespace Data.Repositories.UserRoles
{
    public class RoleEntity : Entity
    {
        public string Name { get; set; }

        public virtual IEnumerable<UserRoleEntity> UserRoles { get; set; }
    }
}
