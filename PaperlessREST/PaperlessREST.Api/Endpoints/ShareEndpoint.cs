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
    }

    private static async Task<Results<Ok<string>, BadRequest<string>, NotFound<string>>> CreateShareLink(
        [FromBody] ShareLink shareLink, IShareService shareService)
    {
        if (shareLink.MetaDataId == null || shareLink.ExpireDate == null || shareLink.Password == null)
            return TypedResults.BadRequest("MetaDataId, ExpireDate and Password are required");
        var shareLinkString = await shareService.CreateShareLinkAsync(shareLink.MetaDataId, shareLink.Password, shareLink.ExpireDate);
        return TypedResults.Ok(shareLinkString);
    }
}