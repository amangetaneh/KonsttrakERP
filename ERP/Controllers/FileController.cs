using ERP.Services.FileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [AllowAnonymous]
        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            if (User.Identity?.IsAuthenticated != true && !IsPublicTemplate(fileName)) return Challenge();
            try { return await _fileService.GetFile(fileName, this); }
            catch (ArgumentException) { return BadRequest("Invalid file name."); }
            catch (FileNotFoundException) { return NotFound(); }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            try { return Ok(await _fileService.SaveFile(file)); }
            catch (ArgumentException) { return BadRequest("Invalid file name."); }
        }

        [AllowAnonymous]
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (User.Identity?.IsAuthenticated != true && !IsPublicTemplate(fileName)) return Challenge();
            try { return await _fileService.DownloadFile(fileName, this); }
            catch (ArgumentException) { return BadRequest("Invalid file name."); }
            catch (FileNotFoundException) { return NotFound(); }
        }

        private static bool IsPublicTemplate(string fileName) => fileName is "blank_forms.pdf" or "blank_forms.docx";
    }
}
