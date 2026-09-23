using Microsoft.EntityFrameworkCore;
using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public class ShareRepository(DbContext context) : IShareRepository
{
    public async Task<string> CreateShareLinkAsync(string metaDataId, string password, DateTime expirationDate)
    {
        var link = new ShareLink {
            Password = password,
            ExpireDate = expirationDate,
            DocumentId = metaDataId
        };
        context.ShareLink.Add(link);
        await context.SaveChangesAsync();
        return link.Guid;
    }

    public async Task<ShareLink?> GetLinkAsync(string shareLinkGuid)
    {
        var shareLink = await context.ShareLink
            .FirstOrDefaultAsync(s => s.Guid == shareLinkGuid);
        return shareLink;
    }
}