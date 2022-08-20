using AutoMapper;
using Domain.Models;

namespace Data.Repositories.Users
{
    public interface ICreateUser
    {
        Task<User> Execute(string email, string hashedPassword, string emailVerificationToken);
    }

    public class CreateUser : ICreateUser
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CreateUser(
            ApiContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> Execute(string email, string hashedPassword, string emailVerificationToken)
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

            return _mapper.Map<User>(userEntity);
        }
    }

}
