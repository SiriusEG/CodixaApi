using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {

        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {

            _lessonService = lessonService;

        }

        [HttpPost("AddNewLesson")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddNewLesson([FromForm] AddLessonRequestDto AddLessontDto)
        {
            try
            {
                if (ModelState.IsValid) {
                    return BadRequest("Lesson Info Is Empty");
                }
                var result = await _lessonService.addLesson(AddLessontDto);
                return Ok(result);
            }
            catch (InvalidDataEnteredException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (FileUplodingException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "there are error while adding Lesson"+ ex);
            }
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteLesson(DeleteLessonRequestDto model)
        {
            try
            {
                if (ModelState == null)
                {
                    return BadRequest("Lesson Id Is Empty");
                }
                var result = await _lessonService.DeleteLesson(model);
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
