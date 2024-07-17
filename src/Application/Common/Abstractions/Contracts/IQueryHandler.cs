using Domain.Shared;
using MediatR;

namespace Application.Common.Abstractions.Contracts;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}
