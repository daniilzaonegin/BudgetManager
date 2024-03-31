using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Shared;

public record BalanceEntryDto
{
    public int Id { get; set; }

    [Required]
    public string? Description { get; set; }

    public DateTime? EntryDate { get; set; }

    [Required]
    public decimal? Amount { get; set; }

    [Required]
    public CategoryDto? Category { get; set; }
}
