using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public interface IDocumentMetaRepository
{
    Task<MetaData?> GetDocument(string id);
    Task<string> UploadDocument(MetaData metaData);
    Task EditDocument(MetaData metaData);
    Task RemoveDocument(string id);

}