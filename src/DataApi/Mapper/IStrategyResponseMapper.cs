using System.Net;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Models.Response;

namespace DataApi.Mapper;

public interface IStrategyResponseMapper
{
    StrategyResponse Map(ThirdPartyACompanyDataResponse response);
    StrategyResponse Map(ThirdPartyBCompanyDataResponse response);

    StrategyResponse Map(ThirdPartyACompanyDataResponse thirdPartyACompanyDataResponse,
        ThirdPartyBCompanyDataResponse thirdPartyBCompanyDataResponse);
}