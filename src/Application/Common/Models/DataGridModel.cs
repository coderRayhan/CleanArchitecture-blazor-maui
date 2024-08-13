namespace Application.Common.Models;
public abstract record DataGridModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Offset => (PageNumber - 1) * PageSize;

    public string SortField { get; set; } = string.Empty;
    public string SortingDirection { get; set; } = "DESC";
    public string? DefaultOrderFieldName { get; set; } = null;
}
