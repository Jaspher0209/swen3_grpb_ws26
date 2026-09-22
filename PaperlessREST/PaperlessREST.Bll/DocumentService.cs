using PaperlessREST.Dal;
using PaperlessREST.Models;

namespace PaperlessREST.Bll;

public class DocumentService : IDocumentService
{
    private readonly IRepository _documentRepository;
    
    public DocumentService(IRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public Task<bool> PostDocumentAsync(MetaData metaData)
    {
        throw new NotImplementedException();
    }

    public async Task<MetaData> GetDocumentMetadataAsync(string id)
    {
        return await _documentRepository.GetDocument(id);
    }

    public async Task<string> GetDocumentContentAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateDocumentAsync(MetaData metaData)
    {
        throw new NotImplementedException();
        await _documentRepository.EditDocument(metaData);
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