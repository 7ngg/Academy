namespace GroupService.Data.Dtos
{
    public class GroupCreateDto
    {
        public required string Name { get; set; }
        public required Guid FacultyId { get; set; }
        public required Guid TeacherId { get; set; }
    }
}
