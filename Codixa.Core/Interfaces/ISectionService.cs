using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface ISectionService
    {
       Task<List<AddSectionResponse>> addSection(AddSectionRequestDto addSectionDto);
       Task<List<AddSectionResponse>> GetSections(int CourseId);
       Task<string> DeleteSection(DeleteSectionRequestDto deleteSectionRequestDto);
       Task<UpdateSectionNameResDto> UpdateSectionName(UpdateSectionNameReqDto updateSectionNameReqDto);
    }
}
