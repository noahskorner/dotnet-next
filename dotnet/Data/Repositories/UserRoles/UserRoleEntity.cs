using Data.Repositories.Users;

namespace Data.Repositories.UserRoles
{
    public class UserRoleEntity : Auditable
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual RoleEntity Role { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
