﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface IStudentService
    {
        Task<string> RequestToEnrollCourse(int CourseId, String token);
    }
}
