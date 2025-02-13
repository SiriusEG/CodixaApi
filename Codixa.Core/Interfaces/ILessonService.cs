using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.LessonDtos.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface ILessonService
    {
        Task<AddLessonResponseDto> addLesson(AddLessonRequestDto AddLessonDto);
        Task<string> DeleteLesson(DeleteLessonRequestDto deleteLessonRequestDto);

    }
}
