using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;

namespace BudgetManager.Profiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<BalanceEntry, BalanceEntryDto>().ReverseMap();
    }
}
