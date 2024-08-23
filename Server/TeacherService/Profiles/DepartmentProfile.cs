using AutoMapper;
using DataLayer.Models;
using TeacherService.Data;

namespace TeacherService.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
        }
    }
}
