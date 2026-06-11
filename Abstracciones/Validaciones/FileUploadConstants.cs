namespace Abstracciones.Validaciones
{
    public static class FileUploadConstants
    {
        public const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public static readonly HashSet<string> AllowedDocumentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-powerpoint",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "text/plain",
            "text/csv",
            "image/png",
            "image/jpeg"
        };

        public static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/png",
            "image/jpeg",
            "image/gif",
            "image/webp"
        };

        public static bool IsValidFileType(string contentType, HashSet<string> allowedTypes)
        {
            return allowedTypes.Contains(contentType);
        }

        public static bool IsWithinSizeLimit(long fileSize)
        {
            return fileSize <= MaxFileSize;
        }
    }
}
