using Domain.Shared;
using MediatR;

namespace Application.Common.Abstractions.Contracts;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{

}
