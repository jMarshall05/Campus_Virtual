using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Servicios.Servicios
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        public FileStorageService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public Task DeleteAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            var uploadsPath = Path.Combine(_rootPath, "Uploads", folder);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }
    }
}
