using AutoMapper;
using DataLayer.Models;
using DepartmentService.Data;

namespace DepartmentService.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDto>();
        }
    }
}
