namespace React_WebApp_docxBlob
{
    public class BlobResponseDto
    {
        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }
    }
}
