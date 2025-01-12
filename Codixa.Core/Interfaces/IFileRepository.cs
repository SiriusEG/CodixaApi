using Codixa.Core.Models.sharedModels;
using Codxia.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface IFileRepository :IBaseRepository<FileEntity>
    {
        Task<FileEntity> UploadFileAsync(IFormFile file, string Path);
        Task<bool> DeleteExistsFileAsync(string Path);
    }
}
