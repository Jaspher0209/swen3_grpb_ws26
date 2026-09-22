using Models;

namespace PaperlessREST.Api.Dtos;

public class MetaDataDto
{
    public string Id { get; set; }
    public string Filename { get; set; }
    public string Description { get; set; }
    public string Updated { get; set; }
    public string Author { get; set; }
}

public static class MetaDataDtoExtensions
{
    public static MetaDataDto ToDto(this MetaData metaData)
    {
        return new MetaDataDto
        {
            Id = metaData.id,
            Filename = metaData.filename,
            Description = metaData.description,
            Updated = metaData.updated,
            Author = metaData.author
        };
    }
    
    public static MetaData ToModel(this MetaDataDto metaDataDto)
    {
        return new MetaData
        {
            id = metaDataDto.Id,
            filename = metaDataDto.Filename,
            description = metaDataDto.Description,
            updated = metaDataDto.Updated,
            author = metaDataDto.Author
        };
    }
}