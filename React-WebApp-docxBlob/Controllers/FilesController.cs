using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace React_WebApp_docxBlob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            try
            {
                _fileService = fileService;
                Console.WriteLine("good");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [HttpPost]  
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file, string email)
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

    }
}
