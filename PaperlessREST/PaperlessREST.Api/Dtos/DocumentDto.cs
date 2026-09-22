using Models;

namespace PaperlessREST.Api.Dtos;

public class DocumentDto
{
    public string Id { get; set; }
    public string Data { get; set; }
    public MetaDataDto MetaData { get; set; }
}

public static class DocumentDtoExtensions
{
    public static DocumentDto ToDto(this Document document)
    {
        return new DocumentDto
        {
            Id = document.id,
            Data = document.data,
            MetaData = document.metaData.ToDto()
        };
    }
    
    public static Document ToModel(this DocumentDto documentDto)
    {
        return new Document
        {
            id = documentDto.Id,
            data = documentDto.Data,
            metaData = documentDto.MetaData.ToModel()
        };
    }
}