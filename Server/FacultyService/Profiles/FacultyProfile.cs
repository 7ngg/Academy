using AutoMapper;
using DataLayer.Models;
using FacultyService.Data;

namespace FacultyService.Profiles
{
    public class FacultyProfile : Profile
    {
        public FacultyProfile()
        {
            CreateMap<Faculty, FacultyDto>();
            CreateMap<FacultyCreateDto, Faculty>();
        }
    }
}
