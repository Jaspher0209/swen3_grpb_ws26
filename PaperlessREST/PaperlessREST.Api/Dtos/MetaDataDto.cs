using System.Text.Json.Serialization;
using PaperlessREST.Models;

namespace PaperlessREST.Api.Dtos;

public class MetaDataDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    public string Id { get; set; }
    public string Filename { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public string? Author { get; set; }
}

public static class MetaDataDtoExtensions
{
    public static MetaDataDto ToDto(this MetaData metaData)
    {
        return new MetaDataDto
        {
            Id = metaData.Id,
            Filename = metaData.Filename,
            Description = metaData.Description,
            Updated = metaData.Updated,
            Author = metaData.Author
        };
    }
    
    public static MetaData ToModel(this MetaDataDto metaDataDto)
    {
        return new MetaData
        {
            Id = metaDataDto.Id,
            Filename = metaDataDto.Filename,
            Description = metaDataDto.Description,
            Updated = metaDataDto.Updated,
            Author = metaDataDto.Author
        };
    }
}