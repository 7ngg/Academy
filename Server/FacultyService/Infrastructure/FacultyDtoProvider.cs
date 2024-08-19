using DataLayer.Models;
using FacultyService.Data;

namespace FacultyService.Infrastructure
{
    public static class FacultyDtoProvider
    {
        public static FacultyDto Generate(Faculty faculty)
        {
            return new()
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Groups = faculty.Groups is null ? [] :
                    faculty.Groups.Select(g => new GroupDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                    }).ToList(),
            };
        }
    }
}
