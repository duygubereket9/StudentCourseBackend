namespace Domain.Entities
{
    public class Enrollment
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = default!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
