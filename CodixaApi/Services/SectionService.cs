﻿using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Respone;
using Codixa.Core.Dtos.SectionsDtos.TestSection.request;
using Codixa.Core.Enums;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codxia.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
                // Validate input
                if (CourseId == 0) // CourseId is an integer, so it cannot be null but can be 0
                {
                    throw new InvalidDataEnteredException("Course Id is invalid!");
                }

                // Fetch sections for the given course with related data
                var sections = await _unitOfWork.Sections.GetListOfEntitiesByIdIncludesAsync(
                    s => s.CourseId == CourseId,  // Filter condition
                    query => query
                        .Include(s => s.Lessons).ThenInclude(l => l.Video) // Include lessons and videos
                        .Include(s => s.SectionTest) // Include test content if applicable
                );

                // Map sections to the response DTO
                List<AddSectionResponse> sectionResponses = sections
                    .Where(s => s != null) // Ensure no null sections in the collection
                    .Select(s => new AddSectionResponse
                    {
                        SectionId = s.SectionId,
                        SectionName = s.SectionName,
                        SectionOrder = s.SectionOrder,
                        SectionType = s.SectionType,
                        // Populate SectionContent if SectionType is "Section"
                        SectionContent = s.SectionType == SectionTypeEnum.Section && s.Lessons != null
                            ? s.Lessons.Select(lesson => new AddSectionContentDto
                            {
                                lessonId = lesson.LessonId,
                                LessonName = lesson.LessonName,
                                IsVideo = lesson.IsVideo,
                                VideoLink = lesson.IsVideo && lesson.Video != null ? lesson.Video.FilePath : null,
                                LessonText = lesson.IsVideo ? null : lesson.LessonText,
                                LessonOrder = lesson.LessonOrder,
                                IsForpreview = lesson.IsForpreview
                            })
                            .OrderBy(lesson => lesson.LessonOrder)
                            .ToList()
                            : null,

                        // Populate TestContent if SectionType is "Test"
                        TestContent = s.SectionType == SectionTypeEnum.Test && s.SectionTest != null
                            ? new AddSectionTestContentDto
                            {
                                SectionTestId = s.SectionTest.SectionTestId,
                                SuccessResult = s.SectionTest.SuccessResult
                            }
                            : null

                    })
                    .OrderBy(x => x.SectionOrder)
                    .ToList();

                return sectionResponses;
            }
            catch (InvalidDataEnteredException)
            {
                throw; // Rethrow the specific exception
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while fetching sections: " + ex.Message);
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
                Section section = new Section();
                if (addSectionDto.SectionType == SectionTypeEnum.Test)
                {

                    section.SectionName = addSectionDto.SectionName;
                    section.CourseId = addSectionDto.CourseId;
                    section.SectionOrder = addSectionDto.SectionOrder;
                    section.SectionType = SectionTypeEnum.Test;
                  
                }
                else
                {
                    section.SectionName = addSectionDto.SectionName;
                    section.CourseId = addSectionDto.CourseId;
                    section.SectionOrder = addSectionDto.SectionOrder;
                    section.SectionType = SectionTypeEnum.Section;
                }

         

      
                await _unitOfWork.Sections.AddAsync(section);
                await _unitOfWork.Complete();


                var sections = await _unitOfWork.Sections.GetListOfEntitiesByIdIncludesAsync(
                    s => s.CourseId == addSectionDto.CourseId,
                    query => query.Include(s => s.Lessons).ThenInclude(l => l.Video)
                                  .Include(s => s.SectionTest)
                );

                // Map sections to the response DTO
                List<AddSectionResponse> sectionResponses = sections.Select(s => new AddSectionResponse
                {
                    SectionId = s.SectionId,
                    SectionName = s.SectionName,
                    SectionOrder = s.SectionOrder,
                    SectionType=s.SectionType,
                    SectionContent = s.SectionType == SectionTypeEnum.Section && s.Lessons != null
                    ? s.Lessons.Select(lesson => new AddSectionContentDto
                     {
                         LessonName = lesson.LessonName,
                         IsVideo = lesson.IsVideo,
                         VideoLink = lesson.IsVideo && lesson.Video != null ? lesson.Video.FilePath : null,
                         LessonText = lesson.IsVideo ? null : lesson.LessonText,
                         LessonOrder = lesson.LessonOrder,
                         IsForpreview = lesson.IsForpreview
                     }).OrderBy(lesson => lesson.LessonOrder).ToList()
                     : null,

                    TestContent = s.SectionType == SectionTypeEnum.Test && s.SectionTest != null
                    ? new AddSectionTestContentDto
                    {
                        SectionTestId = s.SectionTest.SectionTestId,
                        SuccessResult = s.SectionTest.SuccessResult
                    }
                    : null

                }).OrderBy(x => x.SectionOrder).ToList();

                return sectionResponses;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("There was an error while adding the section: " + ex.Message);
            }
        }

        public async Task<string> DeleteSection(DeleteSectionRequestDto deleteSectionRequestDto)
        {
            try
            {
                // Step 1: Find the section to delete
                var section = await _unitOfWork.Sections.FirstOrDefaultAsync(x => x.SectionId == deleteSectionRequestDto.SectionId);
                if (section == null)
                {
                    throw new Exception("Section Not Found!");
                }

                if (section.SectionType == SectionTypeEnum.Test)
                {
                    // Step 2: Find and delete related SectionTests and Questions
                    var sectionTests = await _unitOfWork.SectionTests.GetEntitesListBy(st => st.SectionId == section.SectionId);
                    foreach (var sectionTest in sectionTests)
                    {
                        // Delete all questions related to the section test
                        var questions = await _unitOfWork.Questions.GetEntitesListBy(q => q.SectionTestId == sectionTest.SectionTestId);
                        await _unitOfWork.Questions.DeleteRangeAsync(questions);

                        // Delete the section test itself
                        await _unitOfWork.SectionTests.DeleteAsync(sectionTest);
                    }
                }
                else
                {
                    var sectionLessons = await _unitOfWork.Lessons.GetEntitesListBy(st => st.SectionId == section.SectionId);
            
                        // Delete the section test itself
                     await _unitOfWork.Lessons.DeleteRangeAsync(sectionLessons);
                   
                }
                // Step 3: Delete the section
                await _unitOfWork.Sections.DeleteAsync(section);
                await _unitOfWork.Complete();

                return "Section Deleted Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while deleting the section: " + ex.Message);
            }
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



            return new UpdateSectionNameResDto { SectionName = updateSectionNameReqDto.NewSectionName };

        }

        public async Task<int> UpdateSectionsAndLessonsAsync(List<UpdateSectionLessonNameOrderdto> sectionsToUpdate)
        {
            try
            {
                if (sectionsToUpdate == null || sectionsToUpdate.Count == 0)
                {
                    throw new ArgumentException("No sections provided for update.");
                }

                
                var sectionTable = new DataTable();
                sectionTable.Columns.Add("SectionId", typeof(int));
                sectionTable.Columns.Add("SectionOrder", typeof(int));
                sectionTable.Columns.Add("SectionName", typeof(string));

                
                var lessonTable = new DataTable();
                lessonTable.Columns.Add("LessonId", typeof(int));
                lessonTable.Columns.Add("SectionId", typeof(int));
                lessonTable.Columns.Add("LessonOrder", typeof(int));
                lessonTable.Columns.Add("LessonName", typeof(string));

               
                foreach (var section in sectionsToUpdate)
                {
                    sectionTable.Rows.Add(
                        section.SectionId,
                        section.SectionOrder,
                        section.SectionName
                    );

                    if (section.Lessons != null)
                    {
                        foreach (var lesson in section.Lessons)
                        {
                            lessonTable.Rows.Add(
                                lesson.LessonId,
                                section.SectionId,
                                lesson.LessonOrder,
                                lesson.LessonName
                            );
                        }
                    }
                }

               
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

                
                var updatedSections = await _unitOfWork.ExecuteStoredProcedureAsyncIntReturnScalar(
                    "UpdateSectionsAndLessons", sectionParam, lessonParam
                );


                


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


        //exam Section models
        public async Task<AddNewTestDto> GetTestBySectionId(int sectionId)
        {
            // Get section test with its questions and choices
            var sectionTest = await _unitOfWork.SectionTests.FirstOrDefaultAsync(
                st => st.SectionId == sectionId,
                st => st.Include(x => x.Questions).ThenInclude(q => q.ChoicesQuestions)
            );

            if (sectionTest == null)
                throw new Exception($"No test found for section ID: {sectionId}");

            var testDto = new AddNewTestDto
            {
                SectionId = sectionTest.SectionId,
                SuccessResult = sectionTest.SuccessResult,
                MaxAttempts = sectionTest.MaxAttempts,
                Questions = sectionTest.Questions.Select(q => new AddNewQuestionDto
                {
                    Text = q.Text,
                    ChoiceAnswer = q.ChoicesQuestions.Select(c => new AddNewChoiceAnswerDto
                    {
                        ChoicesQuestionText = c.ChoicesQuestionText,
                        IsTrue = c.IsTrue
                    }).ToList()
                }).ToList()
            };

            return testDto;
        }

        public async Task<string> AddOrUpdateTest(AddNewTestDto addNewTestDto)
        {
            try
            {
                var section = await _unitOfWork.Sections.GetByIdAsync(addNewTestDto.SectionId);
                if (section == null)
                    throw new Exception($"Section with ID {addNewTestDto.SectionId} not found.");

                var sectionTest = await _unitOfWork.SectionTests.FirstOrDefaultAsync(
                    x => x.SectionId == section.SectionId,
                    x => x.Include(st => st.Questions).ThenInclude(q => q.ChoicesQuestions)
                );

                if (sectionTest == null)
                {
                    // ➕ Create new SectionTest
                    sectionTest = new SectionTest
                    {
                        SectionId = section.SectionId,
                        SuccessResult = addNewTestDto.SuccessResult,
                        MaxAttempts = addNewTestDto.MaxAttempts,
                        Questions = new List<Question>()
                    };

                    _unitOfWork.SectionTests.AddAsync(sectionTest);
                }
                else
                {
                    // 🛠 Update existing SectionTest properties
                    sectionTest.SuccessResult = addNewTestDto.SuccessResult;
                    sectionTest.MaxAttempts = addNewTestDto.MaxAttempts;
                }

                var updatedQuestions = new List<Question>();

                foreach (var dtoQuestion in addNewTestDto.Questions)
                {
                    // Find existing question (by text) or create new
                    var existingQuestion = sectionTest.Questions
                        .FirstOrDefault(q => q.Text == dtoQuestion.Text);

                    if (existingQuestion == null)
                    {
                        // ➕ New question
                        existingQuestion = new Question
                        {
                            Text = dtoQuestion.Text,
                            ChoicesQuestions = new List<ChoicesQuestion>()
                        };
                        sectionTest.Questions.Add(existingQuestion);
                    }

                    // Clear and re-add choices (simple strategy)
                    existingQuestion.ChoicesQuestions.Clear();

                    foreach (var dtoChoice in dtoQuestion.ChoiceAnswer)
                    {
                        existingQuestion.ChoicesQuestions.Add(new ChoicesQuestion
                        {
                            ChoicesQuestionText = dtoChoice.ChoicesQuestionText,
                            IsTrue = dtoChoice.IsTrue
                        });
                    }

                    updatedQuestions.Add(existingQuestion);
                }

                // 🗑 Remove questions that were deleted
                var questionsToRemove = sectionTest.Questions
                    .Where(q => !updatedQuestions.Any(u => u.Text == q.Text))
                    .ToList();

                foreach (var toRemove in questionsToRemove)
                {
                    sectionTest.Questions.Remove(toRemove);
                }

                await _unitOfWork.Complete();

                return sectionTest.SectionTestId > 0 ? "Section Test Updated" : "Section Test Added";
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing test: " + ex.Message);
            }
        }




    }
}
