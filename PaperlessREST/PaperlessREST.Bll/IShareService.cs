using PaperlessREST.Models;

namespace PaperlessREST.Bll;

public interface IShareService {
    Task<string> CreateShareLinkAsync(string link, string password, DateTime expirationDate);
    Task<MetaData> ResolveDocumentMetadataFromLinkAsync(string id, string password);
    Task<Document> ResolveDocumentContentFromLinkAsync(string id, string password);
}