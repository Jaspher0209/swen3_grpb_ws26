using PaperlessREST.Dal;
using PaperlessREST.Models;

namespace PaperlessREST.Bll;

// TODO: Check if services are correct

public class ShareService : IShareService
{
    private readonly IDocumentService _documentService;
    private readonly IDocumentMetaRepository _metaDocumentMetaRepository;
    private readonly IShareRepository _shareRepository;
    
    public ShareService(IDocumentService documentService, IDocumentMetaRepository metaDocumentMetaRepository, IShareRepository shareRepository)
    {
        _documentService = documentService;
        _metaDocumentMetaRepository = metaDocumentMetaRepository;
        _shareRepository = shareRepository;
    }

    public async Task<string> CreateShareLinkAsync(string id, string password, DateTime expirationDate)
    {
        if (await _metaDocumentMetaRepository.GetDocument(id) == null)
            throw new KeyNotFoundException("Document not found");
        if (expirationDate < DateTime.UtcNow) throw new ArgumentException("Expiration date cannot be in the past");
        return await _shareRepository.CreateShareLinkAsync(id, password, expirationDate);
    }

    public async Task<MetaData> ResolveDocumentMetadataFromLinkAsync(string link, string password)
    {
        return await _documentService.GetDocumentMetadataAsync(await ResolveLink(link, password));
    }

    public async Task<string> ResolveDocumentContentFromLinkAsync(string link, string password)
    {
        return await _documentService.GetDocumentContentAsync(await ResolveLink(link, password));
    }

    private async Task<string> ResolveLink(string link, string password)
    {
        var linkData = await _shareRepository.ResolveMetaDataFromLinkAsync(link, password);
        if (linkData == null) throw new KeyNotFoundException("Share link not found");
        if (linkData.Password != null && linkData.Password != password)
            throw new UnauthorizedAccessException("Invalid password");
        if (linkData.ExpireDate < DateTime.UtcNow) throw new UnauthorizedAccessException("Share link expired");
        return linkData.MetaDataId;
    }
}