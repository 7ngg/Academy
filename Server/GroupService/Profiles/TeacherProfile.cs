using AutoMapper;
using DataLayer.Models;
using GroupService.Data.Dtos;

namespace GroupService.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDTO>();
        }
    }
}
