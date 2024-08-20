namespace FacultyService.Services
{
    public interface IFacultyService
    {
        Task Rename(Guid id, string newName);
        Task AddGroup(Guid facultyId, Guid groupId);
    }
}
