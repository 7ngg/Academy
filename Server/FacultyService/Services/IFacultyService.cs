using DataLayer.Models;
using FacultyService.Data;
using FacultyService.Data.Dtos;

namespace FacultyService.Services
{
    public interface IFacultyService
    {
        Task<(Faculty? updated, Error? error)> Rename(Guid id, string newName);
        Task<Faculty> CreateFaculty(FacultyCreateDto newFaculty);
        Task<(Faculty? created, Error? error)> AddGroup(Guid facultyId, Guid groupId);
        Task<(Faculty? updated, Error? error)> RemoveGroup(Guid facultyId, Guid groupId);
    }
}
