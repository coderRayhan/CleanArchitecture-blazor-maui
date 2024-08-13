using Application.Common.Abstractions.Contracts;

namespace Application.Features.Lookups.Commands
{
    public sealed record UpdateLookupCommand(
        Guid Id,
        string Name,
        string NameBN,
        string Code,
        string Description,
        bool Status,
        Guid? ParentId = null)
        : ICacheInvalidatorCommand
    {
        public string CacheKey => throw new NotImplementedException();
    }
}
