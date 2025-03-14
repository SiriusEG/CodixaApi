namespace Codixa.Core.Dtos.SearchDtos
{
    public class SearchCoursesResopnseDto
    {
        public List<SearchCoureInfoDto> Courses { get; set; }
        public int TotalPages { get; set; }
    }
}
