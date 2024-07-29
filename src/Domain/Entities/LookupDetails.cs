using Domain.Common;

namespace Domain.Entities;
public class LookupDetails : AuditableEntityBase
{
    public int LookupId { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameBN { get; set; } = string.Empty;
    public bool Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Dev_Code { get; set; }
}
