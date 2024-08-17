namespace GroupService.Data
{
    public class AddGroupDTO
    {
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
