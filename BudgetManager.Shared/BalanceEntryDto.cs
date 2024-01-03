using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Shared;

public record BalanceEntryDto(int Id, [Required] string Description, DateTime EntryDate, bool IsExpense, [Required] decimal Amount);
