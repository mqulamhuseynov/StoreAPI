using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstracts
{
    public interface IFileService
    {
        Task<string> UploadFile(Stream filestream, string fileName, string folderName);
        Task  DeleteFile(string fileUrl);
    }
}
