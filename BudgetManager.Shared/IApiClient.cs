using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Shared;

public interface IApiClient
{
    Task<SearchEntriesResult> GetBalanceEntries(DateTime? from = null, DateTime? to = null, int rowsCount = 20);
}
