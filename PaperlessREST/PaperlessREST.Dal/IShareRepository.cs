using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public interface IShareRepository
{
    public Task<string> CreateShareLinkAsync(string metaDataId, string password, DateTime expirationDate);
    public Task<ShareLink?> ResolveMetaDataFromLinkAsync(string shareLinkGuid, string password);
}