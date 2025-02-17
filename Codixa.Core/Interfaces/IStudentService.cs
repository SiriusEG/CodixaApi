namespace Codixa.Core.Interfaces
{
    public interface IStudentService
    {
        Task<string> RequestToEnrollCourse(int CourseId, String token);
    }
}
