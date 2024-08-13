using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lookups.Queries;
public record LookupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBN { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentId { get; set; } = null;
}
