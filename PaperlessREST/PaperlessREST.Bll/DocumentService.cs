using PaperlessREST.Dal;
using PaperlessREST.Models;

namespace PaperlessREST.Bll;

public class DocumentService : IDocumentService
{
    private readonly IDocumentMetaRepository _documentDocumentMetaRepository;
    
    public DocumentService(IDocumentMetaRepository documentDocumentMetaRepository)
    {
        _documentDocumentMetaRepository = documentDocumentMetaRepository;
    }

    public Task<string> PostDocumentAsync(MetaData metaData)
    {
        var id = _documentDocumentMetaRepository.UploadDocument(metaData);
        return id;
    }

    public async Task<MetaData> GetDocumentMetadataAsync(string id)
    {
        return await _documentDocumentMetaRepository.GetDocument(id);
    }

    public async Task<string> GetDocumentContentAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateDocumentAsync(MetaData metaData)
    {
        await _documentDocumentMetaRepository.EditDocument(metaData);
        return true;
    }

    public Task<bool> DeleteDocumentAsync(string id)
    {
        var result = _documentDocumentMetaRepository.RemoveDocument(id);
        return Task.FromResult(true);
    }

    public Task<List<MetaData>> SearchDocumentsAsync(string query)
    {
        throw new NotImplementedException();
    }
}