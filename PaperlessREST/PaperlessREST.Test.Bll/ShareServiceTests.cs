using Moq;
using PaperlessREST.Bll;
using PaperlessREST.Dal;
using PaperlessREST.Models;

namespace PaperlessREST.Test.Bll;

public class ShareServiceTests
{
    [Fact]
    public async Task CreateShareLinkAsync_ShouldReturnLink_WhenDataIsValid()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testId = "001";
        string testPassword = "password";
        DateTime testExpirationDate = DateTime.UtcNow.AddDays(1);
        string expectedLink = "generated-link-123";
        
        mockMetaRepo.Setup(repo => repo.GetDocument(testId))
            .ReturnsAsync(new MetaData { Id = testId });
        
        mockShareRepo.Setup(repo => repo.CreateShareLinkAsync(testId, testPassword, testExpirationDate))
            .ReturnsAsync(expectedLink);
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act
        var result = await shareService.CreateShareLinkAsync(testId, testPassword, testExpirationDate);

        // Assert
        Assert.Equal(expectedLink, result);
        mockMetaRepo.Verify(repo => repo.GetDocument(testId), Times.Once);
        mockShareRepo.Verify(repo => repo.CreateShareLinkAsync(testId, testPassword, testExpirationDate), Times.Once);
    }

    [Fact]
    public async Task CreateShareLinkAsync_ShouldThrowKeyNotFoundException_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testId = "nonexistent-id";
        string testPassword = "password";
        DateTime testExpirationDate = DateTime.UtcNow.AddDays(1);
        
        mockMetaRepo.Setup(repo => repo.GetDocument(testId))
            .ReturnsAsync((MetaData)null);
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => shareService.CreateShareLinkAsync(testId, testPassword, testExpirationDate));
        mockMetaRepo.Verify(repo => repo.GetDocument(testId), Times.Once);
        mockShareRepo.Verify(repo => repo.CreateShareLinkAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
    }

    [Fact]
    public async Task CreateShareLinkAsync_ShouldThrowArgumentException_WhenExpirationDateIsInThePast()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testId = "001";
        string testPassword = "password";
        DateTime testExpirationDate = DateTime.UtcNow.AddDays(-1); // Past date
        
        mockMetaRepo.Setup(repo => repo.GetDocument(testId))
            .ReturnsAsync(new MetaData { Id = testId });
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => shareService.CreateShareLinkAsync(testId, testPassword, testExpirationDate));
        mockMetaRepo.Verify(repo => repo.GetDocument(testId), Times.Once);
        mockShareRepo.Verify(repo => repo.CreateShareLinkAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
    }

    [Fact]
    public async Task ResolveDocumentMetadataFromLinkAsync_ShouldReturnMetaData_WhenLinkIsValid()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testLink = "valid-link";
        string testPassword = "password";
        string testDocumentId = "001";
        var expectedMetaData = new MetaData { Id = testDocumentId, Author = "Test Author" };
        
        mockShareRepo.Setup(repo => repo.GetLinkAsync(testLink))
            .ReturnsAsync(new ShareLink { DocumentId = testDocumentId, Password = testPassword, ExpireDate = DateTime.UtcNow.AddDays(1) });
        
        mockDocumentService.Setup(service => service.GetDocumentMetadataAsync(testDocumentId))
            .ReturnsAsync(expectedMetaData);
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act
        var result = await shareService.ResolveDocumentMetadataFromLinkAsync(testLink, testPassword);

        // Assert
        Assert.Equal(expectedMetaData, result);
        mockShareRepo.Verify(repo => repo.GetLinkAsync(testLink), Times.Once);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync(testDocumentId), Times.Once);
    }

    [Fact]
    public async Task ResolveDocumentMetadataFromLinkAsync_ShouldThrowUnauthorizedAccessException_WhenPasswordIsInvalid()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testLink = "valid-link";
        string testPassword = "wrong-password";
        string testDocumentId = "001";
        
        mockShareRepo.Setup(repo => repo.GetLinkAsync(testLink))
            .ReturnsAsync(new ShareLink { DocumentId = testDocumentId, Password = "correct-password", ExpireDate = DateTime.UtcNow.AddDays(1) });
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => shareService.ResolveDocumentMetadataFromLinkAsync(testLink, testPassword));
        mockShareRepo.Verify(repo => repo.GetLinkAsync(testLink), Times.Once);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ResolveDocumentMetadataFromLinkAsync_ShouldThrowUnauthorizedAccessException_WhenLinkIsExpired()
    {
        // Arrange
        var mockDocumentService = new Mock<IDocumentService>();
        var mockMetaRepo = new Mock<IDocumentMetaRepository>();
        var mockShareRepo = new Mock<IShareRepository>();
        
        string testLink = "valid-link";
        string testPassword = "password";
        string testDocumentId = "001";
        
        mockShareRepo.Setup(repo => repo.GetLinkAsync(testLink))
            .ReturnsAsync(new ShareLink { DocumentId = testDocumentId, Password = testPassword, ExpireDate = DateTime.UtcNow.AddDays(-1) }); // Expired
        
        var shareService = new ShareService(mockDocumentService.Object, mockMetaRepo.Object, mockShareRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => shareService.ResolveDocumentMetadataFromLinkAsync(testLink, testPassword));
        mockShareRepo.Verify(repo => repo.GetLinkAsync(testLink), Times.Once);
        mockDocumentService.Verify(service => service.GetDocumentMetadataAsync(It.IsAny<string>()), Times.Never);
    }
}