using AutoMapper;
using DataLayer.Models;
using TeacherService.Data;

namespace TeacherService.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDto>();
            CreateMap<TeacherCreateDto, Teacher>();
        }
    }
}
