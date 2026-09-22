using PaperlessREST.Models;

namespace PaperlessREST.Bll;

public interface IDocumentService
{
    Task<bool> PostDocumentAsync(MetaData metaData);
    Task<MetaData> GetDocumentMetadataAsync(string id);
    Task<string> GetDocumentContentAsync(string id);
    Task<bool> UpdateDocumentAsync(MetaData metaData);
    Task<bool> DeleteDocumentAsync(string id);
    Task<List<MetaData>> SearchDocumentsAsync(string query);
}