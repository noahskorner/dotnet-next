using AutoMapper;
using Data.Entities.User;

namespace Domain.Features.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserEntity>().ReverseMap();
        }
    }
}
