using Codixa.Core.Models.sharedModels;
using Codxia.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Codixa.Core.Interfaces
{
    public interface IFileRepository :IBaseRepository<FileEntity>
    {
        Task<FileEntity> UploadFileAsync(IFormFile file, string Path);
        Task<bool> DeleteExistsFileAsync(string Path);
    }
}
