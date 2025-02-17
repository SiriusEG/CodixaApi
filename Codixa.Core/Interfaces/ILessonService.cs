using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.LessonDtos.Respone;

namespace Codixa.Core.Interfaces
{
    public interface ILessonService
    {
        Task<AddLessonResponseDto> addLesson(AddLessonRequestDto AddLessonDto);
        Task<string> DeleteLesson(DeleteLessonRequestDto deleteLessonRequestDto);

    }
}
