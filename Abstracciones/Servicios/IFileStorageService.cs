using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Abstracciones.Servicios
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(IFormFile file, string folder);
        Task<Stream> GetAsync(string path);
        Task DeleteAsync(string path);
    }
}
