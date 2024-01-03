using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Data;

public class BalanceEntry
{
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTime EntryDate { get; set; }

    public bool IsExpense { get; set; }

    public decimal Amount { get; set; }
}