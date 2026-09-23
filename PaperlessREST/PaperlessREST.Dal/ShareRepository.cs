using Microsoft.EntityFrameworkCore;
using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public class ShareRepository : IShareRepository
{
    private readonly DbContext _dbContext;

    public ShareRepository(DbContext context)
    {
        _dbContext = context;
    }

    // create share link
    public async Task<string> CreateShareLinkAsync(string metaDataId, string password, DateTime expirationDate)
    {
        throw new NotImplementedException();
        // Check for data existence
        var metaDataExists = await _dbContext.MetaData.AnyAsync(m => m.Id == metaDataId);
        if (!metaDataExists) throw new KeyNotFoundException($"MetaData mit ID {metaDataId} wurde nicht gefunden.");
        /*
        // set meta data id (FK)
        link.MetaDataId = metaDataId;

        // secure guid-creation
        if (string.IsNullOrEmpty(link.Guid))
        {
            link.Guid = Guid.NewGuid().ToString();
        }

        // create entry
        _dbContext.ShareLink.Add(link);
        await _dbContext.SaveChangesAsync();

        return link.Guid;*/
    }

    // resolve metadata from share link
    public async Task<ShareLink?> ResolveMetaDataFromLinkAsync(string shareLinkGuid, string password)
    {
        // load link incl. respective MetaData (.Include)
        var shareLink = await _dbContext.ShareLink
            .Include(s => s.MetaData)
            .FirstOrDefaultAsync(s => s.Guid == shareLinkGuid);

        // case: share link does not exist
        if (shareLink == null) return null;

        // case: expire date
        if (shareLink.ExpireDate < DateTime.UtcNow) return null;

        // case: password is empty and password is wrong
        if (!string.IsNullOrEmpty(shareLink.Password))
            if (shareLink.Password != password)
                return null;

        return shareLink;
    }
}