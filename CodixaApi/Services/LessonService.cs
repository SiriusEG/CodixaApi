using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.LessonDtos.Respone;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.sharedModels;
using Codxia.Core;
using Microsoft.EntityFrameworkCore;

namespace CodixaApi.Services
{
    public class LessonService : ILessonService
    {

        private readonly IUnitOfWork _unitOfWork;
      

        public LessonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
      
        }





        //add New Lesson

        public async Task<AddLessonResponseDto> addLesson(AddLessonRequestDto AddLessonDto)
        {
            FileEntity file = null;
            try
            {
                if(AddLessonDto == null)
                {
                    throw new InvalidDataEnteredException("Lesson Info Is Empty");
                }
              
                if (AddLessonDto.IsVideo && AddLessonDto.Video !=null)
                {
                     file = await _unitOfWork.Files.UploadFileAsync(AddLessonDto.Video, Path.Combine("uploads", "CoursesVideos"));
                    if (file == null)
                    {
                        throw new FileUplodingException("Video Uploading Error!");
                    }
                }
          
                var section = await _unitOfWork.Sections.FirstOrDefaultAsync(x => x.SectionId == AddLessonDto.SectionId);
                
                if(section == null)
                {
                    throw new InvalidDataEnteredException("Section not Found!");
                }
                   
                Lesson lesson = new Lesson
                {
                    LessonName = AddLessonDto.LessonName,
                    IsVideo=AddLessonDto.IsVideo,
                    VideoId=file?.FileId,
                    LessonText=AddLessonDto.LessonText,
                    LessonOrder = AddLessonDto.LessonOrder,
                    IsForpreview = AddLessonDto.IsForpreview,
                    SectionId=AddLessonDto.SectionId
                };

                if (file != null)
                    await _unitOfWork.Files.AddAsync(file);
                await _unitOfWork.Lessons.AddAsync(lesson);
            
                await _unitOfWork.Complete();

                return new AddLessonResponseDto
                {
                    LessonName = lesson.LessonName,
                    IsVideo = lesson.IsVideo,
                    VideoLink = lesson.Video.FilePath,
                    LessonText = lesson.LessonText,
                    LessonOrder = lesson.LessonOrder,
                    IsForpreview = lesson.IsForpreview
                };
            }
            catch (Exception ) {
                if (file != null)
                {
                    await _unitOfWork.Files.DeleteExistsFileAsync(file.FilePath);
                }
                throw;

            }
        }


        //Delete Lesson

        public async Task<string> DeleteLesson(DeleteLessonRequestDto deleteLessonRequestDto)
        {
            // Update Lesson
            try
            {
                try
                {
                    var Lesson = await _unitOfWork.Lessons.FirstOrDefaultAsync(x => x.LessonId == deleteLessonRequestDto.LessonId, x => x.Include(x => x.Video));
                    if (Lesson == null)
                    {
                        throw new Exception("Lesson Not Found!");
                    }
                    await _unitOfWork.Lessons.DeleteAsync(Lesson);
                    await _unitOfWork.Files.DeleteAsync(Lesson.Video);
                    await _unitOfWork.Complete();
                }
                catch (Exception ex) { 
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("there are an error while Deleting Lesson " + ex.Message);
            }



            return "Lesson Deleted Success";

        }









    }
}
