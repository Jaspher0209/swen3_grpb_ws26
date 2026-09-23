using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PaperlessREST.Bll;
using PaperlessREST.Models;

namespace PaperlessREST.Api.Endpoints;

public static class ShareEndpoint
{
    public static void MapShareEndpoint(this IEndpointRouteBuilder builder)
    {
        var baseGroup = builder.MapGroup("/api");
        var shareGroup = baseGroup.MapGroup("/share");

        shareGroup.MapPost("/", CreateShareLink).DisableAntiforgery();
        shareGroup.MapPost("/{link}/metadata", AccessShareLinkMetadata).DisableAntiforgery();
        shareGroup.MapPost("/{link}/content", AccessShareLinkContent).DisableAntiforgery();
    }

    private static async Task<Results<Ok<string>, BadRequest<string>, NotFound<string>>> CreateShareLink(
        [FromBody] ShareLink shareLink, IShareService shareService)
    {
        if (shareLink.DocumentId == null || shareLink.ExpireDate == null || shareLink.Password == null)
            return TypedResults.BadRequest("DocumentId, ExpireDate and Password are required");
        var shareLinkString = await shareService.CreateShareLinkAsync(shareLink.DocumentId, shareLink.Password, shareLink.ExpireDate);
        return TypedResults.Ok(shareLinkString);
    }

    private static async Task<Results<Ok<MetaData>, BadRequest<string>, NotFound<string>>> AccessShareLinkMetadata(
        string link, [FromBody] string password, IShareService shareService)
    {
        if (string.IsNullOrEmpty(link)) return TypedResults.BadRequest("Link is required");
        var metaData = await shareService.ResolveDocumentMetadataFromLinkAsync(link, password);
        return TypedResults.Ok(metaData);
    }
    
    private static async Task<Results<Ok<string>, BadRequest<string>, NotFound<string>>> AccessShareLinkContent(
        string link, [FromBody] string password, IShareService shareService)
    {
        throw new NotImplementedException("AccessShareLinkContent is not implemented yet");
        if (string.IsNullOrEmpty(link)) return TypedResults.BadRequest("Link is required");
        var content = await shareService.ResolveDocumentContentFromLinkAsync(link, password);
        return TypedResults.Ok(content);
    }
}