using DataLayer.Models;
using DepartmentService.Data;

namespace DepartmentService.Services
{
    public interface IDepartmentService
    {
        Task<Department> Create(DepartmentCreateDto newDepartment);
        Task Remove(Guid id);
        Task<Department> Edit(Guid id, DepartmentEditDto data);
        Task<Department> AddTeacher(Guid departmentId, Guid teacherId);
        Task RemoveTeacher(Guid departmentId, Guid teacherId);
    }
}
