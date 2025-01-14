using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Custom_Exceptions
{
    public class RequestIdnotFoundInInstructorJoinRequestsException : Exception
    {
        public RequestIdnotFoundInInstructorJoinRequestsException(string message) : base(message) { }
    }
}
