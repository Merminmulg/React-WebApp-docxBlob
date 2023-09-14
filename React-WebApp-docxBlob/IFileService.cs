namespace React_WebApp_docxBlob
{
    public interface IFileService
    {
        public Task<BlobResponseDto> UploadAsync(IFormFile blob, string email);
    }
}
