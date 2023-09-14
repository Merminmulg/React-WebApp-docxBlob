using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Moq;

namespace React_WebApp_tests
{
    public class FileServiceTest
    {
        [Fact]
        public async Task UploadAsync_ValidFile_ReturnsSuccessResponse()
        {
            // Arrange
            var storageAccount = "blobstoragefortesttasks";
            var containerName = "docx";
            var email = "test@example.com";
            var fileService = new FileService();

            //Moq BlobServiceClient
            var blobServiceClientMock = new Mock<BlobServiceClient>();
            var containerClientMock = new Mock<BlobContainerClient>();

            blobServiceClientMock.Setup(x => x.GetBlobContainerClient(containerName)).Returns(containerClientMock.Object);

            //Moq BlobClient
            var blobClientMock = new Mock<BlobClient>();

            containerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);

            //Moq IFormFile
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.FileName).Returns("example.docx");
            formFileMock.Setup(x => x.ContentType).Returns("application/msword");
            formFileMock.Setup(x => x.OpenReadStream()).Returns(() => new MemoryStream(new byte[] { 0, 1, 2, 3 }));


            // Act
            var response = await fileService.UploadAsync(formFileMock.Object, email);

            // Assert
            Assert.False(response.Error);
            Assert.NotNull(response.Status);
            Assert.NotNull(response.Blob);
            Assert.True(response.Blob.Uri.StartsWith($"https://{storageAccount}.blob.core.windows.net/{containerName}/"));
            Assert.Equal("example.docx", response.Blob.Name);
            // Дополнительные проверки, если необходимо
        }
        [Fact]
        public async Task UploadAsync_InvalidFile_ReturnsSuccessResponse()
        {
            // Arrange
            var storageAccount = "blobstoragefortesttasks";
            var containerName = "docx";
            var email = "test@example.com";
            var fileService = new FileService();

            //Moq BlobServiceClient
            var blobServiceClientMock = new Mock<BlobServiceClient>();
            var containerClientMock = new Mock<BlobContainerClient>();

            blobServiceClientMock.Setup(x => x.GetBlobContainerClient(containerName)).Returns(containerClientMock.Object);

            //Moq BlobClient
            var blobClientMock = new Mock<BlobClient>();

            containerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);

            //Moq IFormFile
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.FileName).Returns("example.txt");
            formFileMock.Setup(x => x.ContentType).Returns("application/msword");
            formFileMock.Setup(x => x.OpenReadStream()).Returns(() => new MemoryStream(new byte[] { 0, 1, 2, 3 }));


            // Act
            var response = await fileService.UploadAsync(formFileMock.Object, email);

            // Assert
            Assert.True(response.Error);
            Assert.NotNull(response.Status);
        }
    }
}