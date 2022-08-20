namespace Data.Entities.User
{
    public interface ICreateUser
    {
        Task<UserEntity> Execute(string email, string hashedPassword, string emailVerificationToken);
    }

    public class CreateUser : ICreateUser
    {
        private readonly ApiContext _context;

        public CreateUser(ApiContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> Execute(string email, string hashedPassword, string emailVerificationToken)
        {
            var userEntity = new UserEntity()
            {
                Email = email,
                Password = hashedPassword,
                EmailVerificationToken = emailVerificationToken,
                IsEmailVerified = false
            };

            await _context.User.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity;
        }
    }

}
