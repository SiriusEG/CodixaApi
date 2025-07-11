using Codixa.Core.Dtos.CertificationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface ICertificationService
    {
        Task<GetCertDto> GetCertification(int CourseId, string token);
        Task<GetCertDto> VerifyCertification(string CertId);
    }
}
