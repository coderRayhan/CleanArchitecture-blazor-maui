using API.Infrastructure;
using Application.Common.Models;
using Application.Features.Lookups.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;

namespace API.Endpoints;

public sealed class Lookups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
            .WithName("GetLookups")
            .Produces<PaginatedList<LookupResponse>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> GetAll(
        ISender sender,
        [FromBody] GetLookupListQuery query)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result.Value);
    }
}
