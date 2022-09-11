using AutoMapper;
using Domain.Models.Users;

namespace Services.Features.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
