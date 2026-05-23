
using Microsoft.AspNetCore.Http;
using Service.Services.Abstracts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Concrates
{
    public class FileService : IFileService
    {
        public Task DeleteFile(string fileUrl)
        {
            throw new NotImplementedException();
        }


        public Task<string> UploadFile(Stream filestream, string folder, string webRootPath, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
