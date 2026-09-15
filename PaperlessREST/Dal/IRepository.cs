using Models;

namespace Dal;

public interface IRepository
{
    Document GetDocument(int id);
    int UploadDocument(Document metaData);
    bool EditDocument(int id, Document metaData);
    bool RemoveDocument(int id);

}