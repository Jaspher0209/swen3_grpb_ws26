using Models;

namespace Dal;

public interface IRepository
{
    Task<MetaData?> GetDocument(int id);
    Task<int> UploadDocument(MetaData metaData);
    Task EditDocument(int id, MetaData metaData);
    Task RemoveDocument(int id);

}