using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public interface IRepository
{
    Task<MetaData?> GetDocument(string id);
    Task<string> UploadDocument(MetaData metaData);
    Task EditDocument(MetaData metaData);
    Task RemoveDocument(string id);

}