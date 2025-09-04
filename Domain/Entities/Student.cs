namespace Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime BirthDate { get; set; }

        public Guid? UserId { get; set; }  // Student ile User'ı eşleştirmek için
        public User? User { get; set; }    // navigation

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
