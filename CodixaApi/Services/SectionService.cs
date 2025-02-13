using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Respone;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codxia.Core;
using Microsoft.EntityFrameworkCore;

namespace CodixaApi.Services
{
    public class SectionService : ISectionService
    {

        private readonly IUnitOfWork _unitOfWork;
        public SectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }


        public async Task<List<AddSectionResponse>> GetSections(int CourseId)
        {
            try
            {
                if (CourseId == null)
                {
                    throw new InvalidDataEnteredException("Course Id is Null!");
                }
                var Secions = await _unitOfWork.Sections.GetListOfEntitiesByIdIncludesAsync(
             s => s.CourseId == CourseId,  // Filter condition
             query => query.Include(s => s.Lessons).ThenInclude(l => l.Video));

                List<AddSectionResponse> sectionResponse = Secions.Select(s => new AddSectionResponse
                {
                    SectionId = s.SectionId,
                    SectionName = s.SectionName,
                    SectionOrder = s.SectionOrder,
                    SectionContent = s.Lessons.Select(lesson => new AddSectionContentDto
                    {
                        LessonName = lesson.LessonName,
                        IsVideo = lesson.IsVideo,
                        VideoLink = lesson.IsVideo ? lesson.Video?.FilePath : null,
                        LessonText = lesson.IsVideo ? null : lesson.LessonText,
                        LessonOrder = lesson.LessonOrder,
                        IsForpreview = lesson.IsForpreview
                    }).ToList()
                }).ToList();

                return sectionResponse;
            }
            catch (InvalidDataEnteredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("there are an error " + ex.Message);
            }

        }

        public async Task<List<AddSectionResponse>> addSection(AddSectionRequestDto addSectionDto)
        {
            try
            {
                if (addSectionDto == null)
                {
                    throw new Exception("Section Info Is Null");
                }

                Section section = new Section
                {
                    SectionName = addSectionDto.SectionName,
                    CourseId = addSectionDto.CourseId,
                    SectionOrder = addSectionDto.SectionOrder

                };

                await _unitOfWork.Sections.AddAsync(section);
                await _unitOfWork.Complete();

                var Secions = await _unitOfWork.Sections.GetListOfEntitiesByIdIncludesAsync(
                    s => s.CourseId == addSectionDto.CourseId,  // Filter condition
                    query => query.Include(s => s.Lessons).ThenInclude(l => l.Video));

                List<AddSectionResponse> sectionResponse = Secions.Select(s => new AddSectionResponse
                {
                    SectionId = s.SectionId,
                    SectionName = s.SectionName,
                    SectionOrder = s.SectionOrder,
                    SectionContent = s.Lessons.Select(lesson => new AddSectionContentDto
                    {
                        LessonName = lesson.LessonName,
                        IsVideo = lesson.IsVideo,
                        VideoLink = lesson.IsVideo ? lesson.Video?.FilePath : null,
                        LessonText = lesson.IsVideo ? null : lesson.LessonText,
                        LessonOrder = lesson.LessonOrder,
                        IsForpreview = lesson.IsForpreview
                    }).ToList()
                }).ToList();

                return sectionResponse;

            }
            catch (Exception ex)
            {
                throw new Exception("there are error while adding Section " + ex.Message);

            }

        }

        public async Task<string> DeleteSection(DeleteSectionRequestDto deleteSectionRequestDto)
        {
            try
            {
                try
                {
                    var Section = await _unitOfWork.Sections.FirstOrDefaultAsync(x => x.SectionId == deleteSectionRequestDto.SectionId);
                    if (Section == null)
                    {
                        throw new Exception("Section Not Found!");
                    }
                    await _unitOfWork.Sections.DeleteAsync(Section);
                    await _unitOfWork.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("there are an error while Deleting Section " + ex.Message);
            }



            return "Section Deleted Success";

        }

        public async Task<UpdateSectionNameResDto> UpdateSectionName(UpdateSectionNameReqDto updateSectionNameReqDto)
        {
            try
            {
                try
                {
                    var Section = await _unitOfWork.Sections.FirstOrDefaultAsync(x => x.SectionId == updateSectionNameReqDto.SectionId);
                    if (Section == null)
                    {
                        throw new Exception("Section Not Found!");
                    }
                    Section.SectionName = updateSectionNameReqDto.NewSectionName;
                    await _unitOfWork.Sections.UpdateAsync(Section);
                    await _unitOfWork.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("there are an error while Updating Section " + ex.Message);
            }



            return new UpdateSectionNameResDto {SectionName= updateSectionNameReqDto.NewSectionName };

        }
    }
}
