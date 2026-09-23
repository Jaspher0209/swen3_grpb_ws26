using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PaperlessREST.Api.Dtos;
using PaperlessREST.Api.Endpoints;
using PaperlessREST.Bll;
using PaperlessREST.Models;

namespace PaperlessREST.Test.Api;

public class DocumentEndpointTests
{
    [Fact]
    public async Task CreateDocument_ShouldReturnCreated_WhenDocumentIsCreated()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        
        string expectedId = "001";
        mockDocumentService.Setup(service => service.PostDocumentAsync(It.IsAny<MetaData>()))
            .ReturnsAsync(expectedId);
        
        var mockFile = new Mock<IFormFile>();
        
        var metaDataDto = new MetaDataDto
        {
            Author = "Max",
            Description = "Test"
        };
        string metadataJson = JsonSerializer.Serialize(metaDataDto);

        // Act
        var result = await DocumentEndpoint.CreateDocument(mockFile.Object, metadataJson, mockDocumentService.Object);

        // Assert
        var createdResult = result as Created<string>;
        Assert.NotNull(createdResult);
        Assert.Equal(expectedId, createdResult.Value);
        mockDocumentService.Verify(service => service.PostDocumentAsync(It.Is<MetaData>(m => m.Author == "Max" && m.Description == "Test")), Times.Once);
    }

    [Fact]
    public async Task CreateDocument_ShouldReturnBadRequest_WhenMetadataIsMissing()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockFile = new Mock<IFormFile>();

        // Act
        var result = await DocumentEndpoint.CreateDocument(mockFile.Object, null, mockDocumentService.Object);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        Assert.NotNull(badRequestResult);
        Assert.Equal("Metadata is required", badRequestResult.Value);
        mockDocumentService.Verify(service => service.PostDocumentAsync(It.IsAny<MetaData>()), Times.Never);
    }
    
    [Fact]
    public async Task GetDocumentMetadata_ShouldReturnOk_WhenDocumentExists()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var expectedMetaData = new MetaData
        {
            Id = "001",
            Author = "Max",
            Description = "Test"
        };
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("001"))
            .ReturnsAsync(expectedMetaData);

        // Act
        var result = await DocumentEndpoint.GetDocumentMetadata("001", mockDocumentService.Object);

        // Assert
        var okResult = result.Result as Ok<MetaDataDto>;
        Assert.NotNull(okResult);
        Assert.Equal(expectedMetaData.Author, okResult.Value.Author);
        Assert.Equal(expectedMetaData.Description, okResult.Value.Description);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("001"), Times.Once);
    }

    [Fact]
    public async Task GetDocumentMetadata_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("999"))
            .ReturnsAsync((MetaData)null);

        // Act
        var result = await DocumentEndpoint.GetDocumentMetadata("999", mockDocumentService.Object);

        // Assert
        var notFoundResult = result.Result as NotFound<string>;
        Assert.NotNull(notFoundResult);
        Assert.Equal("Document not found", notFoundResult.Value);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("999"), Times.Once);
    }
    
    [Fact]
    public async Task UpdateDocument_ShouldReturnOk_WhenDocumentIsUpdatedSuccessfully()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var existingMetaData = new MetaData
        {
            Id = "001",
            Author = "Max",
            Description = "Test"
        };
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("001"))
            .ReturnsAsync(existingMetaData);
        mockDocumentService.Setup(service => service.UpdateDocumentAsync(It.IsAny<MetaData>()))
            .ReturnsAsync(true);

        var mockFile = new Mock<IFormFile>();
        var updatedMetaDataDto = new MetaDataDto
        {
            Author = "Max Updated",
            Description = "Test Updated"
        };
        string metadataJson = JsonSerializer.Serialize(updatedMetaDataDto);

        // Act
        var result = await DocumentEndpoint.UpdateDocument("001", mockFile.Object, metadataJson, mockDocumentService.Object);

        // Assert
        var okResult = result.Result as Ok<string>;
        Assert.NotNull(okResult);
        Assert.Equal("Document updated successfully", okResult.Value);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("001"), Times.Once);
        mockDocumentService.Verify(service => service.UpdateDocumentAsync(It.Is<MetaData>(m => m.Id == "001" && m.Author == "Max Updated" && m.Description == "Test Updated")), Times.Once);
    }

    [Fact]
    public async Task UpdateDocument_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("999"))
            .ReturnsAsync((MetaData)null);

        var mockFile = new Mock<IFormFile>();
        var updatedMetaDataDto = new MetaDataDto
        {
            Author = "Max Updated",
            Description = "Test Updated"
        };
        string metadataJson = JsonSerializer.Serialize(updatedMetaDataDto);

        // Act
        var result = await DocumentEndpoint.UpdateDocument("999", mockFile.Object, metadataJson, mockDocumentService.Object);

        // Assert
        var notFoundResult = result.Result as NotFound<string>;
        Assert.NotNull(notFoundResult);
        Assert.Equal("Document not found", notFoundResult.Value);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("999"), Times.Once);
        mockDocumentService.Verify(service => service.UpdateDocumentAsync(It.IsAny<MetaData>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDocument_ShouldReturnBadRequest_WhenMetadataIsMissing()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var existingMetaData = new MetaData
        {
            Id = "001",
            Author = "Max",
            Description = "Test"
        };
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("001"))
            .ReturnsAsync(existingMetaData);

        var mockFile = new Mock<IFormFile>();

        // Act
        var result = await DocumentEndpoint.UpdateDocument("001", mockFile.Object, null, mockDocumentService.Object);

        // Assert
        var badRequestResult = result.Result as BadRequest<string>;
        Assert.NotNull(badRequestResult);
        Assert.Equal("Metadata is required", badRequestResult.Value);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("001"), Times.Once);
        mockDocumentService.Verify(service => service.UpdateDocumentAsync(It.IsAny<MetaData>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteDocument_ShouldReturnOk_WhenDocumentIsDeletedSuccessfully()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("001"))
            .ReturnsAsync(new MetaData { Id = "001" });
        mockDocumentService.Setup(service => service.DeleteDocumentAsync("001"))
            .ReturnsAsync(true);

        // Act
        var result = await DocumentEndpoint.DeleteDocument("001", mockDocumentService.Object);

        // Assert
        var okResult = result.Result as Ok<string>;
        Assert.NotNull(okResult);
        Assert.Equal("Document deleted successfully", okResult.Value);
        mockDocumentService.Verify(service => service.DeleteDocumentAsync("001"), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync("999"))
            .ReturnsAsync((MetaData)null);

        // Act
        var result = await DocumentEndpoint.DeleteDocument("999", mockDocumentService.Object);

        // Assert
        var notFoundResult = result.Result as NotFound<string>;
        Assert.NotNull(notFoundResult);
        Assert.Equal("Document not found", notFoundResult.Value);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync("999"), Times.Once);
        mockDocumentService.Verify(service => service.DeleteDocumentAsync(It.IsAny<string>()), Times.Never);
    }
}