using Dal;
using Models;

namespace PaperlessREST.Bll;

public class DocumentMetaService : IDocumentMetaService
{
    private readonly IRepository _documentRepository;
    
    public DocumentMetaService(IRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<MetaData> GetDocumentMetadataAsync(string id)
    {
        throw new NotImplementedException();
        return await _documentRepository.GetDocument(id);
    }

    public async Task<bool> UpdateDocumentAsync(Document document)
    {
        throw new NotImplementedException();
        await _documentRepository.EditDocument(document.id, document.metaData); // data missing, only metadata is updated
        return true;
    }
}