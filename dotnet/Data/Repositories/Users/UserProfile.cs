using AutoMapper;
using Data.Repositories.UserRoles;
using Domain.Models.Users;

namespace Data.Repositories.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RoleEntity, Role>();
            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(x => x.UserRoles.Select(y => y.Role).ToList()));
        }
    }
}
