using Microsoft.EntityFrameworkCore;
using Models;

namespace Dal;

public class DocumentMetaRepository : IRepository
{
    private readonly MetadataDbContext _dbContext;

    public DocumentMetaRepository(MetadataDbContext dbContext)
    {
        _dbContext = dbContext; 
    }
    
    
    // Retrieve Document from Db
    public async Task<MetaData?> GetDocument(string id)
    {
        var metaData = await _dbContext.MetaData.FirstOrDefaultAsync(m => m.id == id);
        return metaData;
    }

    // Upload Document into Db

    public async Task<string> UploadDocument(MetaData metaData)
    {
        _dbContext.MetaData.Add(metaData);
        await _dbContext.SaveChangesAsync();
        return metaData.id;
    }

    // Edit Document in Db
    public async Task EditDocument(string id, MetaData metaData)
    {
        var found = await _dbContext.MetaData.FindAsync(id);

        if (found == null)
            return;

        found.author = metaData.author;
        found.description = metaData.description;
        found.filename = metaData.filename;
        found.updated = metaData.updated;

        await _dbContext.SaveChangesAsync();
    }

    // Remove Document from Db
    public async Task RemoveDocument(string id)
    {
        var found = await _dbContext.MetaData.FindAsync(id);
        if (found != null)
        {
            _dbContext.MetaData.Remove(found);
            await _dbContext.SaveChangesAsync();
        }
    }
}