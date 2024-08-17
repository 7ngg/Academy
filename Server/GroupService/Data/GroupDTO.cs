namespace GroupService.Data
{
    public class GroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FacultyDTO Faculty { get; set; }
        public TeacherDTO Teacher { get; set; }
        public List<StudentDTO> Students { get; set; } = [];
    }
}
