using Models;

namespace PaperlessREST.Bll;

public interface IDocumentService
{
    Task<bool> PostDocumentAsync(Document document);
    Task<MetaData> GetDocumentMetadataAsync(string id);
    Task<string> GetDocumentContentAsync(string id);
    Task<bool> UpdateDocumentAsync(Document document);
    Task<bool> DeleteDocumentAsync(string id);
    Task<List<MetaData>> SearchDocumentsAsync(string query);
}