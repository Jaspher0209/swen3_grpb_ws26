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
    public async Task<MetaData?> GetDocument(int id)
    {
        var products = await _dbContext.MetaData.FirstOrDefaultAsync();
        return products;
    }

    // Upload Document into Db

    public async Task<int> UploadDocument(MetaData metaData)
    {
        _dbContext.MetaData.Add(metaData);
        await _dbContext.SaveChangesAsync();
        return metaData.id;
    }

    // Edit Document in Db
    public async Task EditDocument(int id, MetaData metaData)
    {
        var found = await _dbContext.MetaData.FindAsync(id);
        
        if (found != null)
        {
            found = metaData;
            await _dbContext.SaveChangesAsync(); // Auto-detects and applies changes!
        }
    }

    // Remove Document from Db
    public async Task RemoveDocument(int id)
    {
        var found = await _dbContext.MetaData.FindAsync(id);
        if (found != null)
        {
            _dbContext.MetaData.Remove(found);
            await _dbContext.SaveChangesAsync();
        }
    }
}