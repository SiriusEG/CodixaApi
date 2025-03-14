namespace Codixa.Core.Dtos.CourseDto.CourseDetailsDtos
{
    public class SectionsCourseDetailsDto
    {
        public string SectionName { get; set; }
        public string SectionType { get; set; }
        public List<LessonsSectionsCourseDetails> Lessons { get; set; }
    }
}
