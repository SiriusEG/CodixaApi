namespace Codixa.Core.Dtos.CategoriesDto.request
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; }
    }
}
