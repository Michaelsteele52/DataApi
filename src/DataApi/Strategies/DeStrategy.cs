using DataApi.Configuration;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Mapper;
using DataApi.Models.Response;

namespace DataApi.Strategies;

public class DeStrategy : IJurisdictionStrategy
{ 
    public int OrderNumber => (int)JurisdictionStrategyOrder.De;
    private readonly IThirdPartyAClient _thirdPartyAClient;
    private readonly IThirdPartyBClient _thirdPartyBClient;
    private readonly IStrategyResponseMapper _mapper;
    private readonly IEnumerable<int>? _supportedCompanyIds;
    private readonly bool _companyIdValidationFlag;
    
    public DeStrategy(IThirdPartyAClient thirdPartyAClient, IThirdPartyBClient thirdPartyBClient, IStrategyResponseMapper mapper, CompanyIdConfiguration companyIdConfiguration)
    {
        _thirdPartyAClient = thirdPartyAClient;
        _thirdPartyBClient = thirdPartyBClient;
        _mapper = mapper;
        _companyIdValidationFlag = companyIdConfiguration.CompanyIdValidation;
        _supportedCompanyIds = companyIdConfiguration.CompanyIdJurisdictionDictionary?[JurisdictionStrategyOrder.De.ToString()];
    }
    public bool IsSupported(string jurisdictionCode, int companyId)
    {
        var deJurisdictionCode = JurisdictionStrategyOrder.De.ToString().ToLowerInvariant();

        if(!string.Equals(deJurisdictionCode, jurisdictionCode, StringComparison.OrdinalIgnoreCase)) return false;

        return !_companyIdValidationFlag || _supportedCompanyIds!.Contains(companyId);
    }

    public async Task<StrategyResponse> GetInfo(int companyId, CancellationToken cancellationToken)
    {
        var thirdPartyAResponse =
            await _thirdPartyAClient.GetCompanyData(
                JurisdictionStrategyOrder.De.ToString().ToLower(),
                companyId,
                cancellationToken);
        
        var thirdPartyBResponse =
            await _thirdPartyBClient.GetCompanyData(
                JurisdictionStrategyOrder.De.ToString(),
                companyId,
                cancellationToken);

        return _mapper.Map(thirdPartyAResponse, thirdPartyBResponse);
    }
}