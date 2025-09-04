namespace Application.Students;

public class UpdateStudentDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime BirthDate { get; set; }
}
