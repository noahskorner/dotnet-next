using Microsoft.EntityFrameworkCore;

namespace Data.Entities.User
{
    public interface IGetUserByEmail
    {
        Task<UserEntity?> Execute(string email);
    }

    public class GetUserByEmail : IGetUserByEmail
    {
        private readonly ApiContext _context;

        public GetUserByEmail(ApiContext context)
        {
            _context = context;
        }

        public Task<UserEntity?> Execute(string email)
        {
            return _context.User.SingleOrDefaultAsync(x => x.Email == email);
        }
    }


}
