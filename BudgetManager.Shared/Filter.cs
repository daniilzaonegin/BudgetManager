namespace BudgetManager.Shared;

public class Filter
{
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int RowCount { get; init; } = 20;
}