namespace BudgetManager.Shared;

public class Filter
{
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int StartIndex { get; set; } = 0;
    public int RowCount { get; set; } = 20;
    public string? SortBy { get; set; }
    public string Order { get; set; } = "asc";
}