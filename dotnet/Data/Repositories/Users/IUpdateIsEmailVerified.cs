using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Users
{
    public interface IUpdateIsEmailVerified
    {
        Task<User> Execute(long userId, bool isEmailVerified);
    }

    public class UpdateIsEmailVerified : IUpdateIsEmailVerified
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public UpdateIsEmailVerified(
            ApiContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> Execute(long userId, bool isEmailVerified)
        {
            var userEntity = await _context.User.SingleAsync(x => x.Id == userId);
            userEntity.IsEmailVerified = isEmailVerified;
            await _context.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }
    }
}
