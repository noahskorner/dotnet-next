namespace Data.Entities.User
{
    public interface ICreateUser
    {
        Task<UserEntity> Execute(UserEntity userEntity);
    }

    public class CreateUser : ICreateUser
    {
        private readonly ApiContext _context;

        public CreateUser(ApiContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> Execute(UserEntity user)
        {

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }

}
