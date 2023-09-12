using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace React_WebApp_docxBlob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(FileService fileService, ILogger<FilesController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost]  
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file, string email)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File not selected or empty");

                }
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(fileName).ToLower();

                if (fileExtension == ".docx")
                {
                    var result = await _fileService.UploadAsync(file, email);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Invalid file format");
                }
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError($"HTTP {ex.StatusCode} Error: {ex.Message}");
                return new StatusCodeResult((int)ex.StatusCode);
            }
        }

    }
}
