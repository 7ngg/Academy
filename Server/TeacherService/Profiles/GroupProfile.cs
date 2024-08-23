using AutoMapper;
using DataLayer.Models;
using TeacherService.Data;

namespace TeacherService.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>();
        }
    }
}
