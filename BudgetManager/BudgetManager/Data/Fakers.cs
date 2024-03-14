using Bogus;

namespace BudgetManager.Data;

public class Fakers
{
    private Faker<BalanceEntry> _balanceEntryFaker;
    private Faker<Category> _categoryFaker;
    private readonly string[] fakeCategoryNames = ["Lebensmittel", "Kultur", "Restaurants", "Bildung", "Nebenkosten", "Haushalt", "Reisen"];
    public Faker<BalanceEntry> GetBalanceEntryFaker()
    {
        if (_balanceEntryFaker is null)
        {
            _balanceEntryFaker = new Faker<BalanceEntry>()
                .RuleFor(c => c.EntryDate, f => f.Date.Between(DateTime.Now.AddMonths(-2), DateTime.Now))
                .RuleFor(c => c.Amount, f => f.Random.Decimal(-100.00m, 100.00m))
                .RuleFor(c => c.Description, f => f.Lorem.Sentence(4));
        }

        return _balanceEntryFaker;
    }

    public Faker<Category> GetCategoryFaker()
    {
        if (_categoryFaker is null)
        {
            _categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.PickRandom(fakeCategoryNames));
        }

        return _categoryFaker;
    }
}
