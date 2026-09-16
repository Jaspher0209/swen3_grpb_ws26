using PaperlessREST.Models;

namespace PaperlessREST.Bll;

public interface IDocumentService
{
    Task<Document> GetDocumentByIdAsync(string id);
}