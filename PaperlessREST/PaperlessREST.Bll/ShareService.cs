using System.Diagnostics;
using Models;

namespace PaperlessREST.Bll;

// TODO: Check if services are correct

public class ShareService(IShareRepository shareRepository, IMetaRepository metaRepository, IDocumentService documentService) : IShareService {
    private readonly IShareRepository _shareRepository = shareRepository;
    private readonly IMetaRepository _metaRepository = metaRepository;
    private readonly IDocumentService _documentService = documentService;
    
    public async Task<string> CreateShareLinkAsync(string id, string password, DateTime expirationDate) {
        if (await _metaRepository.GetDocumentAsync(id) == null) throw new KeyNotFoundException("Document not found");
        if (expirationDate < DateTime.UtcNow) throw new ArgumentException("Expiration date cannot be in the past");
        return _shareRepository.CreateShareLink(id, password, expirationDate);
    }
    
    public Task<MetaData> ResolveDocumentMetadataFromLinkAsync(string link, string password) {
        return _documentService.GetDocumentMetadataAsync(ResolveLink(link, password));
    }
    
    public Task<Document> ResolveDocumentContentFromLinkAsync(string link, string password) {
        return _documentService.GetDocumentContentAsync(ResolveLink(link, password));
    }
    
    private string ResolveLink(string link, string password) {
        var linkData = _shareRepository.GetShareLink(link);
        if (linkData == null) throw new KeyNotFoundException("Share link not found");
        if (linkData.Password != null && linkData.Password != password) throw new UnauthorizedAccessException("Invalid password");
        if (linkData.ExpirationDate < DateTime.UtcNow) throw new UnauthorizedAccessException("Share link expired");
        return linkData.DocumentId;
    }
}