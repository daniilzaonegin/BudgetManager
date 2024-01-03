using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Shared;

public class SearchEntriesResult
{
    public int TotalRowCount { get; init; }

    public BalanceEntryDto[] Items { get; init; } = Array.Empty<BalanceEntryDto>();
}
