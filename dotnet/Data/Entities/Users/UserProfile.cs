using AutoMapper;
using Domain.Models;

namespace Data.Entities.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>();
        }
    }
}
