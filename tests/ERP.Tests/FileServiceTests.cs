using ERP.Services.FileServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ERP.Tests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task SavesSyntheticUploadWithUniqueNameAndReadsSameBytes()
        {
            var root = Path.Combine(Path.GetTempPath(), "erp-files-" + Guid.NewGuid());
            try
            {
                var service = new FileService(new TestEnvironment { ContentRootPath = root });
                using var stream = new MemoryStream([1, 2, 3, 4]);
                var file = new FormFile(stream, 0, stream.Length, "file", "demo.txt");
                var name = await service.SaveFile(file);
                Assert.StartsWith("file_", name);
                Assert.EndsWith(".txt", name);
                var result = await service.GetFile(name, new TestController());
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, result.FileContents);
                var download = await service.DownloadFile(name, new TestController());
                Assert.Equal(name, download.FileDownloadName);
            }
            finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
        }

        [Theory]
        [InlineData("demo.pdf", "application/pdf")]
        [InlineData("demo.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("demo.unknown-file-type", "application/octet-stream")]
        public async Task DownloadUsesKnownMimeTypesAndASafeFallback(string name, string expectedType)
        {
            var root = Path.Combine(Path.GetTempPath(), "erp-files-" + Guid.NewGuid());
            try
            {
                var service = new FileService(new TestEnvironment { ContentRootPath = root });
                await File.WriteAllBytesAsync(Path.Combine(root, "UploadedFiles", name), [1, 2, 3]);
                var result = await service.DownloadFile(name, new TestController());
                Assert.Equal(expectedType, result.ContentType);
                Assert.Equal(name, result.FileDownloadName);
            }
            finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
        }

        [Theory]
        [InlineData("../private.txt")]
        [InlineData("..\\private.txt")]
        [InlineData("/private.txt")]
        [InlineData("..")]
        public async Task RejectsPathsOutsideUploadDirectory(string path)
        {
            var root = Path.Combine(Path.GetTempPath(), "erp-files-" + Guid.NewGuid());
            try
            {
                var service = new FileService(new TestEnvironment { ContentRootPath = root });
                await Assert.ThrowsAsync<ArgumentException>(() => service.GetFile(path, new TestController()));
            }
            finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
        }
    }

    internal sealed class TestController : ControllerBase { }
    internal sealed class TestEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "ERP.Tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = "";
        public string EnvironmentName { get; set; } = "Testing";
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
