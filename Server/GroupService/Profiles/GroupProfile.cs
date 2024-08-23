using AutoMapper;
using DataLayer.Models;
using GroupService.Data.Dtos;

namespace GroupService.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDTO>();
            CreateMap<GroupCreateDto, Group>();
        }
    }
}
