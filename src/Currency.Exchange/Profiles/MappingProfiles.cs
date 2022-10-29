using AutoMapper;
using Currency.Exchange.Public.Contracts.Requests;

namespace Currency.Exchange.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<TradeRequest, Data.DbContext.Exchange>();
    }
}