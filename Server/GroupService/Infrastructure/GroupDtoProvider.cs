using DataLayer.Models;
using GroupService.Data;

namespace GroupService.Infrastructure
{
    public static class GroupDtoProvider
    {
        public static GroupDTO Generate(Group group)
        {
            var groupDto = new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                Faculty = new FacultyDTO
                {
                    Id = group.Faculty.Id,
                    Name = group.Faculty.Name
                },
                Teacher = new TeacherDTO
                {
                    Id = group.Teacher.Id,
                    FirstName = group.Teacher.User.FirstName,
                    LastName = group.Teacher.User.LastName,
                },
                Students = group.Students.Select(s => new StudentDTO
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                }).ToList()
            };

            return groupDto;
        }
    }
}
