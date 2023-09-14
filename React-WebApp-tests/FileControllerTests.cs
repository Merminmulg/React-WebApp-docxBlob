using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using React_WebApp_docxBlob.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React_WebApp_tests
{
    public class FileControllerTests
    {
        private readonly Mock<IFileService> fileServiceMock;
        private readonly Mock<ILogger<FilesController>> loggerMock;
        private readonly FilesController controller;

        public FileControllerTests()
        {
            fileServiceMock = new Mock<IFileService>();
            loggerMock = new Mock<ILogger<FilesController>>();

            controller = new FilesController(fileServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task UploadFileAsync_ValidFile_ReturnsOkResult()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(file => file.FileName).Returns("file.docx");
            var email = "test@example.com";

            fileServiceMock.Setup(service => service.UploadAsync(It.IsAny<IFormFile>(), email))
                .ReturnsAsync(new BlobResponseDto
                {
                    Status = "File uploaded successfully",
                    Error = false,
                    Blob = new BlobDto
                    {
                        Uri = "https://example.com/file.docx",
                        Name = "file.docx"
                    }
                });

            // Act
            var result = await controller.UploadFileAsync(formFile.Object, email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<BlobResponseDto>(okResult.Value);

            Assert.False(response.Error);
            Assert.Equal("File uploaded successfully", response.Status);
            Assert.NotNull(response.Blob);
            Assert.Equal("https://example.com/file.docx", response.Blob.Uri);
            Assert.Equal("file.docx", response.Blob.Name);
        }

        [Fact]
        public async Task UploadFileAsync_InvalidFileFormat_ReturnsBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(file => file.FileName).Returns("file.txt");
            var email = "test@example.com";

            fileServiceMock.Setup(service => service.UploadAsync(It.IsAny<IFormFile>(), email))
                .ReturnsAsync(new BlobResponseDto
                {
                    Status = "File uploaded successfully",
                    Error = false,
                    Blob = new BlobDto
                    {
                        Uri = "https://example.com/file.docx",
                        Name = "file.docx"
                    }
                });

            // Act
            var result = await controller.UploadFileAsync(formFile.Object, email);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid file format", badRequestResult.Value);
        }
        [Fact]
        public async Task UploadFileAsync_FileIsNull_ReturnsBadRequest()
        {
            // Arrange
            IFormFile formFile = null;
            var email = "test@example.com";

            fileServiceMock.Setup(service => service.UploadAsync(It.IsAny<IFormFile>(), email))
                .ReturnsAsync(new BlobResponseDto
                {
                    Status = "File uploaded successfully",
                    Error = false,
                    Blob = new BlobDto
                    {
                        Uri = "https://example.com/file.docx",
                        Name = "file.docx"
                    }
                });

            // Act
            var result = await controller.UploadFileAsync(formFile, email);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File not selected", badRequestResult.Value);
        }
    }
}
