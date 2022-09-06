using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Users
{
    public interface IGetUserByEmail
    {
        Task<User> Execute(string email);
    }

    public class GetUserByEmail : IGetUserByEmail
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public GetUserByEmail(
            ApiContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> Execute(string email)
        {
            var userEntity = await _context.User
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == email);

            return _mapper.Map<User>(userEntity);
        }
    }
}
