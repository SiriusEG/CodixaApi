using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Interfaces;
using CodixaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {

        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService)
        {

            _sectionService = sectionService;

        }



        [HttpGet("GetAllSections/{CourseId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetAllSections([FromRoute] int CourseId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pass the token and DTO to the service class
                    var result = await _sectionService.GetSections(CourseId);
                    return Ok(result);
                }catch (InvalidDataEnteredException ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }

                catch (Exception ex)
                {
                    return StatusCode(500,  ex);
                }
            }

            return BadRequest("Invalid Data Request!");
        }


        
        [HttpPost("AddNewSection")]
        [Authorize(Roles = "Instructor")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddNewSection([FromBody] AddSectionRequestDto addSectionDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pass the token and DTO to the service class
                    var result = await _sectionService.addSection(addSectionDto);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }

            return BadRequest("Invalid Data Request!");
        }


        [HttpPut("UpdateSectionName")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateSectionName(UpdateSectionNameReqDto model)
        {
            try
            {
                if (ModelState == null)
                {
                    return BadRequest("Section Id Is Empty");
                }
                var result = await _sectionService.UpdateSectionName(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteSection(DeleteSectionRequestDto model)
        {
            try
            {
                if (ModelState == null)
                {
                    return BadRequest("Section Id Is Empty");
                }
                var result = await _sectionService.DeleteSection(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPut("UpdateSectionsLessons")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateSectionsAndLessons([FromBody] List<UpdateSectionLessonNameOrderdto> sectionsToUpdate)
        {
            try
            {
                var updatedSections = await _sectionService.UpdateSectionsAndLessonsAsync(sectionsToUpdate);

                return Ok(new { message = "Sections and lessons updated successfully."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating sections and lessons.", error = ex.Message });
            }
        }

    }
}
