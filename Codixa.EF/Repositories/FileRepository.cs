using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.sharedModels;
using Codxia.EF;
using Codxia.EF.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.EF.Repositories
{
    public class FileRepository : BaseRepository<FileEntity>, IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public FileRepository(AppDbContext context, IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<FileEntity> UploadFileAsync(IFormFile file, string Location)
        {
            FileEntity fileEntity = new FileEntity();
            if (file != null)
            {
                try
                {
                    var path = Path.Combine(_environment.WebRootPath, Location);

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var extension = Path.GetExtension(file.FileName);
                   
                    var fileName = fileEntity.FileId + extension;

                    using (FileStream fileStream = System.IO.File.Create(Path.Combine(path, fileName)))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }
             
                    fileEntity.FileName = fileName;
                    fileEntity.FilePath = Path.Combine(Location, fileName);
                }
                catch 
                {
                    throw new FileUplodingException("File Uploading Failed");
                }
            }
            return fileEntity;
        }



        public Task<bool> DeleteExistsFileAsync(string path)
        {
            var filePath = Path.Combine(_environment.WebRootPath, path);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
