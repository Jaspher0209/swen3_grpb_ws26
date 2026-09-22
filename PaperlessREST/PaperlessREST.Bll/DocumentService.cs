using Dal;
using Models;

namespace PaperlessREST.Bll;

public class DocumentService : IDocumentService
{
    private readonly IRepository _documentRepository;
    
    public DocumentService(IRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public Task<bool> PostDocumentAsync(Document document)
    {
        throw new NotImplementedException();
    }

    public async Task<MetaData> GetDocumentMetadataAsync(string id)
    {
        throw new NotImplementedException();
        return await _documentRepository.GetDocument(id);
    }

    public async Task<string> GetDocumentContentAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateDocumentAsync(Document document)
    {
        throw new NotImplementedException();
        await _documentRepository.EditDocument(document.id, document.metaData); // data missing, only metadata is updated
        return true;
    }

    public Task<bool> DeleteDocumentAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<MetaData>> SearchDocumentsAsync(string query)
    {
        throw new NotImplementedException();
    }
}