using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Data;

public class BalanceEntry
{
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTime EntryDate { get; set; }

    public decimal Amount { get; set; }

    public Category Category { get; set; }

    public int CategoryId { get; set; }
}