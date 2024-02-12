using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;

namespace BudgetManager.Profiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<BalanceEntry, BalanceEntryDto>().ReverseMap();
    }
}
