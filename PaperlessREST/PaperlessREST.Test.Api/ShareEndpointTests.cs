using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PaperlessREST.Api.Endpoints;
using PaperlessREST.Bll;
using PaperlessREST.Models;

namespace PaperlessREST.Test.Api;

public class ShareEndpointTests
{
    [Fact]
    public async Task CreateShareLink_ShouldReturnOk_WhenShareLinkIsCreated()
    {
        // Arrange
        var mockShareService = new Mock<IShareService>();
        var shareLink = new ShareLink
        {
            DocumentId = "001",
            Password = "password",
            ExpireDate = DateTime.UtcNow.AddDays(1)
        };
        string expectedLink = "share-link";
        mockShareService.Setup(service => service.CreateShareLinkAsync(shareLink.DocumentId, shareLink.Password, shareLink.ExpireDate))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await ShareEndpoint.CreateShareLink(shareLink, mockShareService.Object);

        // Assert
        var okResult = result.Result as Ok<string>;
        Assert.NotNull(okResult);
        Assert.Equal(expectedLink, okResult.Value);
        mockShareService.Verify(service => service.CreateShareLinkAsync(shareLink.DocumentId, shareLink.Password, shareLink.ExpireDate), Times.Once);
    }

    [Fact]
    public async Task CreateShareLink_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var mockShareService = new Mock<IShareService>();
        var shareLink = new ShareLink
        {
            DocumentId = null,
            Password = null
        };

        // Act
        var result = await ShareEndpoint.CreateShareLink(shareLink, mockShareService.Object);

        // Assert
        var badRequestResult = result.Result as BadRequest<string>;
        Assert.NotNull(badRequestResult);
        Assert.Equal("DocumentId, ExpireDate and Password are required", badRequestResult.Value);
        mockShareService.Verify(service => service.CreateShareLinkAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
    }

    [Fact]
    public async Task AccessShareLinkMetadata_ShouldReturnOk_WhenMetadataIsResolved()
    {
        // Arrange
        var mockShareService = new Mock<IShareService>();
        string link = "share-link";
        string password = "password";
        var expectedMetaData = new MetaData
        {
            Id = "001",                                                                                                                                                                                                                                                                                                              
            Filename = "document.pdf",                                                                                                                                                                                                                                                                                               
            Author = "Max Muster"
        };
        mockShareService.Setup(service => service.ResolveDocumentMetadataFromLinkAsync(link, password))
            .ReturnsAsync(expectedMetaData);

        // Act
        var result = await ShareEndpoint.AccessShareLinkMetadata(link, password, mockShareService.Object);

        // Assert
        var okResult = result.Result as Ok<MetaData>;
        Assert.NotNull(okResult);
        Assert.Equal(expectedMetaData, okResult.Value);
        mockShareService.Verify(service => service.ResolveDocumentMetadataFromLinkAsync(link, password), Times.Once);
    }

    [Fact]
    public async Task AccessShareLinkMetadata_ShouldReturnBadRequest_WhenLinkIsMissing()
    {
        // Arrange
        var mockShareService = new Mock<IShareService>();
        string link = null;
        string password = "password";

        // Act
        var result = await ShareEndpoint.AccessShareLinkMetadata(link, password, mockShareService.Object);

        // Assert
        var badRequestResult = result.Result as BadRequest<string>;
        Assert.NotNull(badRequestResult);
        Assert.Equal("Link is required", badRequestResult.Value);
        mockShareService.Verify(service => service.ResolveDocumentMetadataFromLinkAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}