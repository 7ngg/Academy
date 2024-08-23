using AutoMapper;
using DataLayer.Models;
using GroupService.Data.Dtos;

namespace GroupService.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDTO>();
        }
    }
}
