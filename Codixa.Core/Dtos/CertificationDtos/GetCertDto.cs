using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.CertificationDtos
{
    public class GetCertDto
    {
        public string CertificationId { get; set; }
        public string StudntName { get; set; }
        public string InstructorName { get; set; }
        public string CourseName { get; set; }
        public string CertificationDate { get; set; }
    }
}
