﻿namespace Codixa.Core.Dtos.SectionsDtos.Respone
{
    public class AddSectionResponse
    {
        public int SectionId { get; set; }
        public int SectionOrder { get; set; }
        public string SectionName { get; set; }

        public List<AddSectionContentDto>? SectionContent { get; set; }
    }
}
