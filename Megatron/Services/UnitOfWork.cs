using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UnitOfWork(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async void UploadImage(IFormFile file)
        {
            var fileName = file.FileName.Trim('"');
            fileName = EnsureFileName(fileName);
            var buffer = new byte[16 * 1024];
            await using var output = System.IO.File.Create(GetPathAndFileName(fileName));
            await using var input = file.OpenReadStream();
            int readBytes;
            while ((readBytes = await input.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await output.WriteAsync(buffer.AsMemory(0, readBytes));
            }
        }

        private string GetPathAndFileName(string fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, fileName);
        }

        private static string EnsureFileName(string fileName)
        {
            if (fileName.Contains("\\"))
                fileName = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            return fileName;
        }
    }
}