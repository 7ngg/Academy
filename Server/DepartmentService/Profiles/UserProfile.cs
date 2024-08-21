using AutoMapper;
using DataLayer.Models;
using DepartmentService.Data;

namespace DepartmentService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
