using AutoMapper;
using DataLayer.Models;
using FacultyService.Data;
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

        public async Task Rename(Guid id, string newName)
        {
            try
            {
                var faculty = await _facultyRepository.GetById(id);
            
                if (faculty.Name.Equals(newName))
                {
                    return;
                }

                faculty.Name = newName;

                await _facultyRepository.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Faculty> CreateFaculty(FacultyCreateDto newFaculty)
        {
            var faculty = _mapper.Map<Faculty>(newFaculty);

            await _facultyRepository.AddAsync(faculty);
            await _facultyRepository.Save();

            return faculty;
        }

        public async Task<Faculty> AddGroup(Guid facultyId, Guid groupId)
        {
            var faculty = await _facultyRepository.GetById(facultyId) 
                ?? throw new NullReferenceException("Faculty does not exist");

            var group = await _groupRepository.GetById(groupId)
                ?? throw new NullReferenceException("Group does not exist");

            group.FacultyId = faculty.Id;
            group.Faculty = faculty;
            faculty.Groups.Add(group);

            await _facultyRepository.Save();

            return faculty;
        }

        public async Task RemoveGroup(Guid facultyId, Guid groupId)
        {
            var faculty = await _facultyRepository.GetById(facultyId)
                ?? throw new NullReferenceException("Faculty does not exist");

            var group = await _groupRepository.GetById(groupId)
                ?? throw new NullReferenceException("Group does not exist");

            faculty.Groups.Remove(group);

            await _facultyRepository.Save();
        }
    }
}
