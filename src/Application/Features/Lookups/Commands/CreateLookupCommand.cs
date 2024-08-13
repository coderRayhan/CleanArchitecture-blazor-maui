using Application.Common.Abstractions;
using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Contracts;
using Domain.Shared;
using System.Text.Json.Serialization;

namespace Application.Features.Lookups.Commands;
public sealed record CreateLookupCommand(
    string Name,
    string NameBN,
    string Code,
    string Description,
    bool Status,
    Guid? ParentId = null)
    : ICacheInvalidatorCommand<Guid>
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.Lookups;
}

internal sealed class CreateLookupCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateLookupCommand, Guid>
{
    public Task<Result<Guid>> Handle(CreateLookupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
