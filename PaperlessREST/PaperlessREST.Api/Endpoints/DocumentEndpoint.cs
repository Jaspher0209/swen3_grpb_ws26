using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PaperlessREST.Api.Dtos;
using PaperlessREST.Bll;

namespace PaperlessREST.Api.Endpoints;

public static class DocumentEndpoint
{
    public static void MapDocumentEndpoint(this IEndpointRouteBuilder builder)
    {
        var baseGroup = builder.MapGroup("/api");
        var documentGroup = baseGroup.MapGroup("/document");
        var searchGroup = documentGroup.MapGroup("/search");
        var shareGroup = documentGroup.MapGroup("/share");

        documentGroup.MapGet("/{id}/metadata", GetDocumentMetadata);
        documentGroup.MapGet("/{id}/content", GetDocumentContent);
        documentGroup.MapPut("/{id}", UpdateDocument);
    }

    private static async Task<Results<Ok<MetaDataDto>, NotFound<string>>> GetDocumentMetadata(string id,
        IDocumentService documentService)
    {
        var documentMetadata = await documentService.GetDocumentMetadataAsync(id);
        if (documentMetadata == null) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok(documentMetadata.ToDto());
    }
    
    private static async Task<Results<Ok<string>, NotFound<string>>> GetDocumentContent(string id,
        IDocumentService documentService)
    {
        var documentContent = await documentService.GetDocumentContentAsync(id);
        if (documentContent == null) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok(documentContent);
    }
    
    private static async Task<Results<Ok<string>, NotFound<string>>> UpdateDocument(string id,
        [FromForm] IFormFile data, [FromForm] string metadata, IDocumentService documentService)
    {
        var document = new DocumentDto()
        {
            Id = id
        };
        
        if (data != null && data.Length > 0)
        {
            var fileName = data.FileName;
            var stream = data.OpenReadStream();
            document.Data = stream.ToString();
        }

        if (!string.IsNullOrEmpty(metadata))
        {
            var metaDataDto = JsonSerializer.Deserialize<MetaDataDto>(metadata);
            if (metaDataDto != null)
            {
                document.MetaData = metaDataDto;
            }
        }

        var updatedMetadata = await documentService.UpdateDocumentAsync(document.ToModel());
        if (updatedMetadata == null) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok("Document updated successfully");
    }
}