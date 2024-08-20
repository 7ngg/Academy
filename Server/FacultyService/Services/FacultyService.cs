using FacultyService.Repositories.Interfaces;

namespace FacultyService.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;

        public FacultyService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
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

        public async Task AddGroup(Guid facultyId, Guid groupId)
        {
            try
            {
                var faculty = _facultyRepository.GetById(facultyId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetGroup(Guid id)
        {

        }
    }
}
