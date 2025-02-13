using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.SectionsDtos.Request
{
    public class UpdateSectionNameReqDto
    {
        public int SectionId { get; set; }
        public string NewSectionName { get; set; }
        
    }
}
