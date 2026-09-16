using PaperlessREST.Models;

namespace PaperlessREST.Api.Dtos;

public class DocumentDto
{
    public string Id { get; set; }
}

public static class DocumentDtoExtensions
{
    public static DocumentDto ToDto(this Document document)
    {
        return new DocumentDto
        {
            Id = document.Id
        };
    }
    
    public static Document ToModel(this DocumentDto documentDto)
    {
        return new Document
        {
            Id = documentDto.Id
        };
    }
}