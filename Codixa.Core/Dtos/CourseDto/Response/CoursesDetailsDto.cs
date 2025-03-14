namespace Codixa.Core.Dtos.CourseDto.Response
{
    public class CoursesDetailsDto 
    {

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public bool IsPublished { get; set; }

        public string CourseCardPhotoPath { get; set; }


        public string CategoryName { get; set; }
    }
}
