using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.CategoriesDto.request
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; }
    }
}
