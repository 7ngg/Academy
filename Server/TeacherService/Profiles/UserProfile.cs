using AutoMapper;
using DataLayer.Models;
using TeacherService.Data;

namespace TeacherService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
