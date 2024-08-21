using DataLayer.Models;
using FacultyService.Data;

namespace FacultyService.Services
{
    public interface IFacultyService
    {
        Task Rename(Guid id, string newName);
        Task<Faculty> CreateFaculty(FacultyCreateDto newFaculty);
        Task<Faculty> AddGroup(Guid facultyId, Guid groupId);
        Task RemoveGroup(Guid facultyId, Guid groupId);
    }
}
