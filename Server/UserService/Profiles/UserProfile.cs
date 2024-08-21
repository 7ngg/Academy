using AutoMapper;
using DataLayer.Models;
using UserService.Data;

namespace UserService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, User>()
                .ForMember(target => target.PasswordHash, opt => opt.MapFrom(source => source.Password));
        }
    }
}
