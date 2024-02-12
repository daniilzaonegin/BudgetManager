using Bogus;

namespace BudgetManager.Data;

public class Fakers
{
    private Faker<BalanceEntry> _balanceEntryFaker;
    public Faker<BalanceEntry> GetBalanceEntryFaker()
    {
        if(_balanceEntryFaker is null)
        {
            _balanceEntryFaker = new Faker<BalanceEntry>()
                .RuleFor(c => c.EntryDate, f => f.Date.Between(DateTime.Now.AddMonths(-2), DateTime.Now))
                .RuleFor(c => c.Amount, f => f.Random.Decimal(-100.00m, 100.00m))
                .RuleFor(c => c.Description, f => f.Lorem.Sentence(4));
        } 

        return _balanceEntryFaker;
    }
}
