namespace Application.Courses;

public class CreateCourseDto
{
    public string Name { get; set; } = default!; //kurs adı
    public string? Description { get; set; } //kurs açıklaması
}
