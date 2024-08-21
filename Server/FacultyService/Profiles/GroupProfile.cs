using AutoMapper;
using DataLayer.Models;
using FacultyService.Data;

namespace FacultyService.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>();
        }
    }
}
