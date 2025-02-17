using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Respone;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codxia.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public async Task<int> UpdateSectionsAndLessonsAsync(List<UpdateSectionLessonNameOrderdto> sectionsToUpdate)
        {
            try
            {
                if (sectionsToUpdate == null || sectionsToUpdate.Count == 0)
                {
                    throw new ArgumentException("No sections provided for update.");
                }

                // Create DataTable for Sections
                var sectionTable = new DataTable();
                sectionTable.Columns.Add("SectionId", typeof(int));
                sectionTable.Columns.Add("SectionOrder", typeof(int));
                sectionTable.Columns.Add("SectionName", typeof(string));

                // Create DataTable for Lessons
                var lessonTable = new DataTable();
                lessonTable.Columns.Add("LessonId", typeof(int));
                lessonTable.Columns.Add("SectionId", typeof(int));
                lessonTable.Columns.Add("LessonOrder", typeof(int));
                lessonTable.Columns.Add("LessonName", typeof(string));

                // Fill DataTables
                foreach (var section in sectionsToUpdate)
                {
                    sectionTable.Rows.Add(
                        section.SectionId,
                        (object?)section.SectionOrder ?? DBNull.Value,
                        (object?)section.SectionName ?? DBNull.Value
                    );

                    if (section.Lessons != null)
                    {
                        foreach (var lesson in section.Lessons)
                        {
                            lessonTable.Rows.Add(
                                lesson.LessonId,
                                section.SectionId,
                                (object?)lesson.LessonOrder ?? DBNull.Value,
                                (object?)lesson.LessonName ?? DBNull.Value
                            );
                        }
                    }
                }

                // Define SQL Parameters
                var sectionParam = new SqlParameter("@SectionUpdates", SqlDbType.Structured)
                {
                    TypeName = "SectionUpdateType",
                    Value = sectionTable
                };

                var lessonParam = new SqlParameter("@LessonUpdates", SqlDbType.Structured)
                {
                    TypeName = "LessonUpdateType",
                    Value = lessonTable
                };

                // Execute Stored Procedure & Read Multiple Results
                var updatedSections = await _unitOfWork.ExecuteStoredProcedureAsyncIntReturn(
                    "UpdateSectionsAndLessons @SectionUpdates, @LessonUpdates", sectionParam, lessonParam
                );

          
                // Group Lessons Inside Their Sections
           

                return updatedSections;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error while updating sections and lessons: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating sections and lessons: " + ex.Message);
            }
        }






    }
}
