
using Microsoft.AspNetCore.Http;
using Service.Services.Abstracts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Domain.Settings;

namespace Service.Services.Concrates
{
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory;
        

        public FileService(IOptions<FileStorageSettings> options)
        {
            _uploadDirectory = options.Value.UploadDirectory;
        }

        public Task DeleteFile(string fileUrl)
        {
            throw new NotImplementedException();
        }



        public async Task<string> UploadFile(Stream filestream, string fileName, string folderName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory, folderName);

            if (!Directory.Exists(imagePath)) 
            {
             Directory.CreateDirectory(imagePath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
            var filePath = Path.Combine(imagePath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create)) 
            {
             await filestream.CopyToAsync(fileStream);
            }

            return $"uploads/{folderName}/{uniqueFileName}";
        }
    }
}
