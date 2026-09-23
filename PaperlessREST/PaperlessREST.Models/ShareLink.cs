using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperlessREST.Models;

public class ShareLink
{
    [Key]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();
    public string Password { get; set; }
    public DateTime ExpireDate { get; set; }
    public string DocumentId { get; set; }
}