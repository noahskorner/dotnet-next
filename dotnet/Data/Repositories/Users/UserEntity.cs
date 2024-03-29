﻿using Data.Repositories.UserRoles;

namespace Data.Repositories.Users
{
    public class UserEntity : Auditable
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailVerificationToken { get; set; }
        public bool IsEmailVerified { get; set; }

        public virtual IEnumerable<UserRoleEntity> UserRoles { get; set; }
    }
}
