using Models;

namespace PaperlessREST.Bll;

public interface IDocumentMetaService
{
    Task<MetaData> GetDocumentMetadataAsync(string id);
    Task<bool> UpdateDocumentAsync(Document document);
}