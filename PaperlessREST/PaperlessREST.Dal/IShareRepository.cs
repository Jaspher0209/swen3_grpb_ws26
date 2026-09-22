using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public interface IShareRepository
{
    public Task<string> CreateShareLinkAsync(string metaDataId, ShareLink link);
    public Task<MetaData?> ResolveMetaDataFromLinkAsync(string shareLinkGuid, string password);
}