using Moq;
using PaperlessREST.Bll;
using PaperlessREST.Dal;
using PaperlessREST.Models;

namespace PaperlessREST.Test.Bll;

public class DocumentServiceTests
{
    [Fact]
    public async Task PostDocument_ShouldCreateNewDocument_WhenValidDataIsProvided()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.UploadDocument(It.IsAny<MetaData>()))
            .ReturnsAsync("001");
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.PostDocumentAsync(new MetaData
        {
            Author = "Test Author",
            Description = "Test Description",
            Filename = "testfile.txt"
        });

        // Assert
        Assert.Equal("001", result);
        mockRepo.Verify(repo => repo.UploadDocument(It.IsAny<MetaData>()), Times.Once);
    }

    [Fact]
    public async Task GetDocumentMetadata_ShouldReturnDocument_WhenDocumentExists()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        var expectedMetaData = new MetaData
        {
            Id = "001",
            Author = "Test Author",
            Description = "Test Description",
            Filename = "testfile.txt"
        };
        mockRepo.Setup(repo => repo.GetDocument("001")).ReturnsAsync(expectedMetaData);
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.GetDocumentMetadataAsync("001");

        // Assert
        Assert.Equal(expectedMetaData, result);
        mockRepo.Verify(repo => repo.GetDocument("001"), Times.Once);
    }

    [Fact]
    public async Task GetDocumentMetadata_ShouldReturnNull_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.GetDocument("999")).ReturnsAsync((MetaData)null);
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.GetDocumentMetadataAsync("999");

        // Assert
        Assert.Null(result);
        mockRepo.Verify(repo => repo.GetDocument("999"), Times.Once);
    }

    [Fact]
    public async Task UpdateDocument_ShouldReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.EditDocument(It.IsAny<MetaData>())).Returns(Task.CompletedTask);
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.UpdateDocumentAsync(new MetaData
        {
            Id = "001",
            Author = "Updated Author",
            Description = "Updated Description",
            Filename = "updatedfile.txt"
        });

        // Assert
        Assert.True(result);
        mockRepo.Verify(repo => repo.EditDocument(It.IsAny<MetaData>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDocument_ShouldReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.EditDocument(It.IsAny<MetaData>())).ThrowsAsync(new Exception("Update failed"));
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.UpdateDocumentAsync(new MetaData
        {
            Id = "001",
            Author = "Updated Author",
            Description = "Updated Description",
            Filename = "updatedfile.txt"
        });

        // Assert
        Assert.False(result);
        mockRepo.Verify(repo => repo.EditDocument(It.IsAny<MetaData>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.RemoveDocument("001")).Returns(Task.CompletedTask);
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.DeleteDocumentAsync("001");

        // Assert
        Assert.True(result);
        mockRepo.Verify(repo => repo.RemoveDocument("001"), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldReturnFalse_WhenDeleteFails()
    {
        // Arrange
        var mockRepo = new Mock<IDocumentMetaRepository>();
        mockRepo.Setup(repo => repo.RemoveDocument("001")).ThrowsAsync(new Exception("Delete failed"));
        
        var documentService = new DocumentService(mockRepo.Object);

        // Act
        var result = await documentService.DeleteDocumentAsync("001");

        // Assert
        Assert.False(result);
        mockRepo.Verify(repo => repo.RemoveDocument("001"), Times.Once);
    }
}