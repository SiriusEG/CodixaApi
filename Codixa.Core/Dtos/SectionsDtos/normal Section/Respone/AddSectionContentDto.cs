namespace Codixa.Core.Dtos.SectionsDtos.Respone
{
    public class AddSectionContentDto
    {
        public int lessonId { get; set; }
        public string LessonName { get; set; }

        public bool IsVideo { get; set; }
        public string? VideoLink { get; set; }

        public string? LessonText { get; set; }

        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; }
    }

}
