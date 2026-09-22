using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperlessREST.Models;

public class ShareLink
{
    [Key]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();
    public string Password { get; set; }
    public DateTime ExpireDate { get; set; }

    // EACH ShareLink has EXACTLY ONE MetaData
    public string MetaDataId { get; set; } = string.Empty;

    [ForeignKey(nameof(MetaDataId))]
    public MetaData MetaData { get; set; } = null!;
}