using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Events
{
    internal sealed class CacheInvalidationEvent : INotification
    {
        public string CacheKey { get; set; } = string.Empty;
    }
}
