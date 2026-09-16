using Microsoft.AspNetCore.Http.HttpResults;
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
        
        documentGroup.MapGet("/{id}", GetDocumentById);
        
        private static async Task<Results<Ok<DocumentDto>, NotFound<string>>> GetDocumentById(string id, IDocumentService documentService)
        {
            var document = await documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return TypedResults.NotFound("Document not found");
            }
            return TypedResults.Ok(document.ToDto());
        }
    }
}