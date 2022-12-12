using DataApi.Configuration;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Mapper;
using DataApi.Models.Response;

namespace DataApi.Strategies;

public class NlStrategy : IJurisdictionStrategy
{ 
    public int OrderNumber => (int)JurisdictionStrategyOrder.Nl;
    private readonly IThirdPartyBClient _thirdPartyBClient;
    private readonly IStrategyResponseMapper _mapper;
    private readonly IEnumerable<int>? _supportedCompanyIds;
    private readonly bool _companyIdValidationFlag;
    
    public NlStrategy(IThirdPartyBClient thirdPartyBClient, IStrategyResponseMapper mapper, CompanyIdConfiguration companyIdConfiguration)
    {
        _thirdPartyBClient = thirdPartyBClient;
        _mapper = mapper;
        _companyIdValidationFlag = companyIdConfiguration.CompanyIdValidation;
        _supportedCompanyIds = companyIdConfiguration.CompanyIdJurisdictionDictionary?[JurisdictionStrategyOrder.Nl.ToString()];
    }
    public bool IsSupported(string jurisdictionCode, int companyId)
    {
        var nlJurisdictionCode = JurisdictionStrategyOrder.Nl.ToString().ToLowerInvariant();

        if(!string.Equals(nlJurisdictionCode, jurisdictionCode, StringComparison.OrdinalIgnoreCase)) return false;
        return !_companyIdValidationFlag || _supportedCompanyIds!.Contains(companyId);
    }

    public async Task<StrategyResponse> GetInfo(int companyId, CancellationToken cancellationToken)
    {
        var response =
            await _thirdPartyBClient.GetCompanyData(
                JurisdictionStrategyOrder.Nl.ToString().ToLower(),
                companyId,
                cancellationToken);

        return _mapper.Map(response);
    }
}