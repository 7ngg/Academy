using DataLayer.Models;
using GroupService.Data;

namespace GroupService.Services
{
    public interface IGroupService
    {
        Group Create(AddGroupDTO request);
        Task<bool> AddStudent(Guid studentId, Guid groupId);
        Task<bool> RemoveStudent(Guid studentId, Guid groupId);
        Task<bool> ChangeTeacher(Guid teacherId, Guid groupId);
        Task<Group> Rename(Guid id, string newName);
    }
}
