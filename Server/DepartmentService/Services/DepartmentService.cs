using AutoMapper;
using DataLayer.Models;
using DepartmentService.Data;
using DepartmentService.Repositories.Interfaces;

namespace DepartmentService.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, ITeacherRepository teacherRepository)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _teacherRepository = teacherRepository;
        }

        public async Task<Department> AddTeacher(Guid departmentId, Guid teacherId)
        {
            var department = await _departmentRepository.GetById(departmentId)
                ?? throw new NullReferenceException();

            var teacher = await _teacherRepository.GetById(teacherId)
                ?? throw new NullReferenceException();

            department.Teachers.Add(teacher);
            await _departmentRepository.SaveAsync();

            return department;
        }

        public async Task<Department> Create(DepartmentCreateDto newDepartment)
        {
            ArgumentNullException.ThrowIfNull(newDepartment, nameof(newDepartment));

            var department = _mapper.Map<Department>(newDepartment);

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveAsync();

            return department;
        }

        public async Task<Department> Edit(Guid id, DepartmentEditDto data)
        {
            ArgumentNullException.ThrowIfNull(data, "Request is null");

            var department = await _departmentRepository.GetById(id)
                ?? throw new ArgumentNullException();

            if (department.Name == data.Name) return null;

            department.Name = data.Name;

            await _departmentRepository.SaveAsync();

            return department;
        }

        public async Task Remove(Guid id)
        {
            var department = await _departmentRepository.GetById(id)
                ?? throw new NullReferenceException();

            _departmentRepository.Delete(department);
            await _departmentRepository.SaveAsync();
        }

        public async Task RemoveTeacher(Guid departmentId, Guid teacherId)
        {
            var department = await _departmentRepository.GetById(departmentId)
                ?? throw new NullReferenceException();

            var teacher = await _teacherRepository.GetById(teacherId)
                ?? throw new NullReferenceException();

            if (!department.Teachers.Contains(teacher)) return;

            department.Teachers.Remove(teacher);
            await _departmentRepository.SaveAsync();
        }
    }
}
