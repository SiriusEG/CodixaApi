using Codixa.Core.Dtos.CategoriesDto.request;
using Codixa.Core.Dtos.CategoriesDto.response;
using Codixa.Core.Models.CourseModels;
using Codxia.Core;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var categoriesDtos = categories.Select(c => new GetCategoriesDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return Ok(categoriesDtos);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewCategory(AddNewCategoryDto categoryDto)
        {
            if (categoryDto != null) { 
                
                Category category = new Category{ Name = categoryDto.Name, Description = categoryDto.Description};
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.Complete();
                return Ok(new NewCategoryResponseDto { CategoryId = category.CategoryId , Name = category.Name , Description = category.Description});
            }
            return BadRequest("there are error while adding Category!");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
        {
            if (categoryDto != null) {

                Category category = await _unitOfWork.Categories.GetByIdAsync(categoryDto.CategoryId);
                category.Name=categoryDto.NewName;
                category.Description= categoryDto.NewDescription;   

                await _unitOfWork.Categories.UpdateAsync(category);

                await _unitOfWork.Complete();
                return Ok(new NewCategoryResponseDto { CategoryId = category.CategoryId, Name = category.Name, Description = category.Description });

            }


            return BadRequest("there are error while Updating Category!");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromBody] int Id)
        {
            if (Id != 0) {
                Category category = await _unitOfWork.Categories.GetByIdAsync(Id);
                await _unitOfWork.Categories.DeleteAsync(category);

                await _unitOfWork.Complete();

                return Ok("Category Deleted Success!");

            }
            return BadRequest("there are error while Deleting Category!");
        }
    }
}
