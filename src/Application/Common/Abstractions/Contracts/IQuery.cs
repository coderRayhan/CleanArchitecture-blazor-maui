using Domain.Shared;
using MediatR;

namespace Application.Common.Abstractions.Contracts;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
