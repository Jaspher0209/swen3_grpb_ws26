using Models;

namespace Dal;

public class DocumentMetaRepository : IRepository
{
    // Retrieve Document from Db
    public Document GetDocument(int id)
    {
        throw new NotImplementedException();
    }

    // Upload Document into Db
    public int UploadDocument(Document metaData)
    {
        throw new NotImplementedException();
    }

    // Edit Document in Db
    
    public bool EditDocument(int id, Document metaData)
    {
        throw new NotImplementedException();
    }

    // Remove Document from Db
    
    public bool RemoveDocument(int id)
    {
        throw new NotImplementedException();
    }
}