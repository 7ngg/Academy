using DataLayer.Models;
using GroupService.Data;
using GroupService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GroupService.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        public GroupService(IGroupRepository groupRepository, IStudentRepository studentRepository, ITeacherRepository teacherRepository)
        {
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }

        public Group Create(AddGroupDTO request)
        {
            var group = new Group()
            {
                Name = request.Name,
                FacultyId = request.FacultyId,
                TeacherId = request.TeacherId,
            };

            return group;
        }

        public async Task<bool> AddStudent(Guid studentId, Guid groupId)
        {
            var updatedStudent = await _studentRepository.GetByIdAsync(studentId);

            if (updatedStudent == null)
            {
                // TODO: Custom exception
                return false;
            }

            var group = await _groupRepository.GetByIdAsync(groupId);

            if (group == null)
            {
                // TODO: Custom exception
                return false;
            }

            try
            {
                group.Students.Add(updatedStudent);

                await _groupRepository.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveStudent(Guid studentId, Guid groupId)
        {
            var updatedStudent = await _studentRepository.GetByIdAsync(studentId);

            if (updatedStudent == null)
            {
                // TODO: Custom exception
                return false;
            }

            var group = await _groupRepository.GetByIdAsync(groupId);

            if (group == null)
            {
                // TODO: Custom exception
                return false;
            }

            try
            {
                group.Students.Remove(updatedStudent);

                await _groupRepository.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangeTeacher(Guid teacherId, Guid groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);

            if (group == null)
            {
                // TODO: Custom exception
                return false;
            }

            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                // TODO: Custom exception
                return false;
            }

            group.TeacherId = teacher.Id;

            await _groupRepository.SaveAsync();

            return true;
        }

        public async Task<Group> Rename(Guid id, string newName)
        {
            var group = await _groupRepository.GetByIdAsync(id);

            if (group == null)
            {
                // TODO: Custom exception
                throw new Exception();
            }

            group.Name = newName;

            await _groupRepository.SaveAsync();

            return group;
        }
    }
}
