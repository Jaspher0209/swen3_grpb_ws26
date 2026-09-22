using Microsoft.EntityFrameworkCore;
using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public class DocumentMetaRepository : IRepository
{
    private readonly DbContext _dbContext;

    public DocumentMetaRepository(DbContext dbContext)
    {
        _dbContext = dbContext; 
    }
    
    
    // Retrieve Document from Db
    public async Task<MetaData?> GetDocument(string id)
    {
        var metaData = await _dbContext.MetaData.FirstOrDefaultAsync(m => m.Id == id);
        return metaData;
    }

    // Upload Document into Db

    public async Task<string> UploadDocument(MetaData metaData)
    {
        _dbContext.MetaData.Add(metaData);
        await _dbContext.SaveChangesAsync();
        return metaData.Id;
    }

    // Edit Document in Db
    public async Task EditDocument(string id, MetaData metaData)
    {
        var found = await _dbContext.MetaData.FindAsync(id);

        if (found == null)
            return;

        found.Author = metaData.Author;
        found.Description = metaData.Description;
        found.Filename = metaData.Filename;
        found.Updated = metaData.Updated;

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