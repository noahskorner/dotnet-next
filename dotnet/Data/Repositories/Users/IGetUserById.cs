﻿using AutoMapper;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Users
{
    public interface IGetUserById
    {
        Task<User> Execute(long userId);
    }

    public class GetUserById : IGetUserById
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public GetUserById(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> Execute(long userId)
        {
            var userEntity = await _context.User
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId);

            return _mapper.Map<User>(userEntity);
        }
    }

}
