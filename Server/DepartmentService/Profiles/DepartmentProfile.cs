using AutoMapper;
using DataLayer.Models;
using DepartmentService.Data;

namespace DepartmentService.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentCreateDto, Department>();
        }
    }
}
