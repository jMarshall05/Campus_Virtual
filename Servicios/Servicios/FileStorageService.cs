using Abstracciones.Servicios;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Servicios.Servicios
{
    public class FileStorageService : IFileStorageService
    {
        // private readonly string _rootPath;
        private readonly Cloudinary _cloudinary;

        public FileStorageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task DeleteAsync(string path)
        {
            //path = path.TrimStart('/');
            //var fullPath = Path.Combine(_rootPath, path);

            //if (!fullPath.StartsWith(_rootPath))
            //    throw new UnauthorizedAccessException("Ruta no permitida");

            //if (!File.Exists(fullPath))
            //    throw new FileNotFoundException($"No se encontró el archivo: {Path.GetFileName(path)}");
            //File.Delete(fullPath);

            //return Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Ruta inválida");

            // Extrae el PublicId de la URL
            var uri = new Uri(path);
            var segments = uri.AbsolutePath.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            var publicIdWithExtension = string.Join("/", segments.Skip(uploadIndex + 2));
            var publicId = Path.ChangeExtension(publicIdWithExtension, null);

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
                throw new Exception($"Error al eliminar archivo: {result.Error.Message}");



        }

        public Task<string> GetAsync(string path)
        {
            //if (string.IsNullOrWhiteSpace(path))
            //    throw new ArgumentException("Ruta inválida");

            //path = path.TrimStart('/');

            //var fullPath = Path.Combine(_rootPath, path);

            //if (!fullPath.StartsWith(_rootPath))
            //    throw new UnauthorizedAccessException("Ruta no permitida");

            //if (!File.Exists(fullPath))
            //    throw new FileNotFoundException($"No se encontró el archivo: {Path.GetFileName(path)}");


            //return Task.FromResult(fullPath);
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Ruta inválida");

            // En Cloudinary el path ya es la URL pública
            return Task.FromResult(path);
        }

        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            //var uploadsPath = Path.Combine(_rootPath, "Uploads", folder);
            //Directory.CreateDirectory(uploadsPath);

            //var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            //var fullPath = Path.Combine(uploadsPath, fileName);

            //using var stream = new FileStream(fullPath, FileMode.Create);
            //await file.CopyToAsync(stream);

            //return Path.Combine("uploads", folder, fileName);

            using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"uploads/{folder}",
                PublicId = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception($"Error al subir archivo: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }
    }
}
