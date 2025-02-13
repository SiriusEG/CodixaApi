using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.SectionsDtos.Request
{
    public class AddSectionRequestDto
    {
        public string SectionName { get; set; }

        public int SectionOrder { get; set; }

        public int CourseId { get; set; }

    }
}
