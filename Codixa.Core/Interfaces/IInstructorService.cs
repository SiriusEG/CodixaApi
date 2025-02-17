﻿using Codixa.Core.Dtos.InstructorDtos.Request;

namespace Codixa.Core.Interfaces
{
    public interface IInstructorService
    {
        Task<object> GetStudentRequestToEnrollCourse(string token, int courseId, int pageNumber, int pageSize);
        Task<object> ChangeStudentRequestStatus(string token, ChangeStudentRequestDto changeStudentRequestDto);
    }
}
