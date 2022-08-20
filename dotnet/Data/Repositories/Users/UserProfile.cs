using AutoMapper;
using Domain.Models;

namespace Data.Repositories.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>();
        }
    }
}
