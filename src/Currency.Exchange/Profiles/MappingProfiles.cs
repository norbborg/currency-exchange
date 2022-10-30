using AutoMapper;
using Currency.Exchange.Public.Contracts.Requests;

namespace Currency.Exchange.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<TradeRequest, Data.DbContext.Exchange>()
            .ForMember(d=>d.Fromcurrency, e => e.MapFrom(s=>s.FromCurrencyCode))
            .ForMember(d=>d.Tocurrency, e => e.MapFrom(s=>s.ToCurrencyCode));
    }
}