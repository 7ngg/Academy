using AutoMapper;
using DataLayer.Models;
using FacultyService.Data;
using FacultyService.Data.Dtos;
using FacultyService.Repositories.Interfaces;

namespace FacultyService.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        private IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public FacultyService(
            IFacultyRepository facultyRepository,
            IMapper mapper,
            IGroupRepository groupRepository)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
            _groupRepository = groupRepository;
        }

        public async Task<(Faculty? updated, Error? error)> Rename(Guid id, string newName)
        {
            var faculty = await _facultyRepository.GetById(id);

            if (faculty is null)
            {
                return (null, new Error(404, "Faculty does not exist"));
            }

            if (faculty.Name.Equals(newName))
            {
                return (faculty, null);
            }

            faculty.Name = newName;

            await _facultyRepository.Save();

            return (faculty, null);
        }

        public async Task<Faculty> CreateFaculty(FacultyCreateDto newFaculty)
        {
            var faculty = _mapper.Map<Faculty>(newFaculty);

            await _facultyRepository.AddAsync(faculty);
            await _facultyRepository.Save();

            return faculty;
        }

        public async Task<(Faculty? created, Error? error)> AddGroup(
            Guid facultyId,
            Guid groupId)
        {
            var faculty = await _facultyRepository.GetById(facultyId);

            if (faculty is null)
            {
                return (
                    null,
                    new Error(404, "Faculty does not exist"));
            }

            var group = await _groupRepository.GetById(groupId);

            if (group is null)
            {
                return (
                    null,
                    new Error(404, "Group does not exist"));
            }

            group.FacultyId = faculty.Id;
            group.Faculty = faculty;
            faculty.Groups.Add(group);

            await _facultyRepository.Save();

            return (faculty, null);
        }

        public async Task<(Faculty? updated, Error? error)> RemoveGroup(Guid facultyId, Guid groupId)
        {
            var faculty = await _facultyRepository.GetById(facultyId);

            if (faculty is null)
            {
                return (
                    null,
                    new Error(404, "Faculty doest not exist"));
            }

            var group = await _groupRepository.GetById(groupId);

            if (group is null)
            {
                return (
                    null,
                    new Error(404, "Group doest not exist"));
            }

            faculty.Groups.Remove(group);

            await _facultyRepository.Save();

            return (faculty, null);
        }
    }
}
