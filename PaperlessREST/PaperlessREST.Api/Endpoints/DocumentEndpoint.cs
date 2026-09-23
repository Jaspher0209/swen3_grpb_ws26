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

        documentGroup.MapPost("/", CreateDocument).DisableAntiforgery();
        documentGroup.MapGet("/{id}/metadata", GetDocumentMetadata);
        documentGroup.MapPut("/{id}", UpdateDocument).DisableAntiforgery();
        documentGroup.MapDelete("/{id}", DeleteDocument);
        searchGroup.MapGet("/", SearchDocuments);
    }
    
    private static async Task<IResult> CreateDocument([FromForm] IFormFile data,
        [FromForm] string metadata, IDocumentService documentService)
    {
        MetaDataDto metaDataDto;
        if (!string.IsNullOrEmpty(metadata))
        {
            var options = new JsonSerializerOptions                                                                                                                           
            {                                                                                                                                                                 
                PropertyNameCaseInsensitive = true                                                                                                                            
            }; 
            metaDataDto = JsonSerializer.Deserialize<MetaDataDto>(metadata, options);
        }
        else
        {
            return TypedResults.BadRequest("Metadata is required");
        }
        
        var id = await documentService.PostDocumentAsync(metaDataDto.ToModel());
        if (string.IsNullOrEmpty(id)) return TypedResults.BadRequest("Failed to create document");
        string? uri = null;
        return TypedResults.Created(uri, id);
    }

    private static async Task<Results<Ok<MetaDataDto>, NotFound<string>>> GetDocumentMetadata(string id,
        IDocumentService documentService)
    {
        var documentMetadata = await documentService.GetDocumentMetadataAsync(id);
        if (documentMetadata == null) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok(documentMetadata.ToDto());
    }
    
    private static async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> UpdateDocument(string id,
        [FromForm] IFormFile data, [FromForm] string metadata, IDocumentService documentService)
    {
        MetaDataDto metaDataDto;
        if (!string.IsNullOrEmpty(metadata))
        {
            var options = new JsonSerializerOptions                                                                                                                           
            {                                                                                                                                                                 
                PropertyNameCaseInsensitive = true                                                                                                                            
            }; 
            metaDataDto = JsonSerializer.Deserialize<MetaDataDto>(metadata, options);
        }
        else
        {
            return TypedResults.BadRequest("Metadata is required");
        }

        var success = await documentService.UpdateDocumentAsync(metaDataDto.ToModel());
        if (!success) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok("Document updated successfully");
    }
    
    private static async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> DeleteDocument(string id, IDocumentService documentService)
    {
        var success = await documentService.DeleteDocumentAsync(id);
        if (!success) return TypedResults.NotFound("Document not found");
        return TypedResults.Ok("Document deleted successfully");
    }
    
    private static async Task<Results<Ok<List<MetaDataDto>>, NotFound<string>, BadRequest<string>>> SearchDocuments([FromQuery] string query, IDocumentService documentService)
    {
        var documents = await documentService.SearchDocumentsAsync(query);
        if (documents == null) return TypedResults.NotFound("Documents not found");
        return TypedResults.Ok(documents.Select(d => d.ToDto()).ToList());
    }
}
