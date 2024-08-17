namespace StudentService.Data
{
    public class EditStudentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid GroupId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
