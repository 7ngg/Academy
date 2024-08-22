namespace FacultyService.Data.Dtos
{
    public class FacultyDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<GroupDto> Groups { get; set; } = [];
    }
}
