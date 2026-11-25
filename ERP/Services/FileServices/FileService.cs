using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ERP.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly string _path;
        private static readonly FileExtensionContentTypeProvider ContentTypes = new();

        public FileService(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "UploadedFiles");
            Directory.CreateDirectory(_path);
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (extension.Length > 16 || extension.Any(c => !char.IsLetterOrDigit(c) && c != '.'))
                throw new ArgumentException("Invalid file extension.", nameof(file));

            var name = $"file_{Guid.NewGuid():N}{extension}";
            await using var stream = new FileStream(GetPath(name), FileMode.CreateNew, FileAccess.Write);
            await file.CopyToAsync(stream);
            return name;
        }

        public async Task<FileContentResult> GetFile(string fileName, ControllerBase controller)
        {
            var file = await File.ReadAllBytesAsync(GetPath(fileName));
            return controller.File(file, GetContentType(fileName));
        }

        public async Task<FileContentResult> DownloadFile(string fileName, ControllerBase controller)
        {
            var file = await File.ReadAllBytesAsync(GetPath(fileName));
            return controller.File(file, GetContentType(fileName), fileName);
        }

        private static string GetContentType(string fileName) =>
            ContentTypes.TryGetContentType(fileName, out var contentType) ? contentType : "application/octet-stream";

        private string GetPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName is "." or ".." || fileName.IndexOfAny(['/', '\\', '\0']) >= 0 || Path.IsPathRooted(fileName))
                throw new ArgumentException("A plain file name is required.", nameof(fileName));
            return Path.Combine(_path, fileName);
        }
    }
}
