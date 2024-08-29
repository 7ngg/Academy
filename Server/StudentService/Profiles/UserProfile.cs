using AutoMapper;
using StudentService.Data;

namespace StudentService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<StudentDTO, UserDTO>();
        }
    }
}
