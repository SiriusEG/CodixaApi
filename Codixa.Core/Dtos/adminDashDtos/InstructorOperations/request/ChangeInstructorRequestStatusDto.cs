using Codixa.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request
{
    public class ChangeInstructorRequestStatusDto : IKeylessEntity
    {
        public int RequestId { get; set; }
        [Length(0,8)]
        public string NewStatus { get; set; }
    }
}
