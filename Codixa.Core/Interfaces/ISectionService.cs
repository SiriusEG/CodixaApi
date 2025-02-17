using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Dtos.SectionsDtos.Respone;

namespace Codixa.Core.Interfaces
{
    public interface ISectionService
    {
       Task<List<AddSectionResponse>> addSection(AddSectionRequestDto addSectionDto);
       Task<List<AddSectionResponse>> GetSections(int CourseId);
       Task<string> DeleteSection(DeleteSectionRequestDto deleteSectionRequestDto);
       Task<UpdateSectionNameResDto> UpdateSectionName(UpdateSectionNameReqDto updateSectionNameReqDto);
       Task<int> UpdateSectionsAndLessonsAsync(List<UpdateSectionLessonNameOrderdto> sectionsToUpdate);
    }
}
