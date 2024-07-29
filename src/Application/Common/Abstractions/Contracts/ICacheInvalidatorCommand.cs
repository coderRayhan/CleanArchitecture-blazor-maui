using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Abstractions.Contracts;
public interface ICacheInvalidatorCommand : ICommand
{
    string CacheKey { get; }
}

public interface ICacheInvalidatorCommand<TResponse> : ICommand<TResponse>
{
    string CacheKey { get; }
}
