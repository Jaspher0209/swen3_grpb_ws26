namespace PaperlessREST.Models;

public class MetaData
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Filename { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public string? Author { get; set; }
    
    // ONE MetaData has MANY ShareLinks
    public ICollection<ShareLink> ShareLinks { get; set; } = new List<ShareLink>();
}