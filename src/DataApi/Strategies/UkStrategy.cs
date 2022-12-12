using DataApi.Configuration;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Mapper;
using DataApi.Models.Response;

namespace DataApi.Strategies;

public class UkStrategy : IJurisdictionStrategy
{
    public int OrderNumber => (int)JurisdictionStrategyOrder.Uk;
    private readonly IThirdPartyAClient _thirdPartyAClient;
    private readonly IStrategyResponseMapper _mapper;
    private readonly IEnumerable<int>? _supportedCompanyIds;
    private readonly bool _companyIdValidationFlag;

    public UkStrategy(IThirdPartyAClient thirdPartyAClient, IStrategyResponseMapper mapper, CompanyIdConfiguration companyIdConfiguration)
    {
        _thirdPartyAClient = thirdPartyAClient;
        _mapper = mapper;
        _companyIdValidationFlag = companyIdConfiguration.CompanyIdValidation;
        _supportedCompanyIds = companyIdConfiguration.CompanyIdJurisdictionDictionary?[JurisdictionStrategyOrder.Uk.ToString()];
    }

    public bool IsSupported(string jurisdictionCode, int companyId)
    {
        var ukJurisdictionCode = JurisdictionStrategyOrder.Uk.ToString().ToLowerInvariant();

        if (!string.Equals(ukJurisdictionCode, jurisdictionCode, StringComparison.OrdinalIgnoreCase)) return false;
        return !_companyIdValidationFlag || _supportedCompanyIds!.Contains(companyId);
    }

    public async Task<StrategyResponse> GetInfo(int companyId, CancellationToken cancellationToken)
    {
        var response = await _thirdPartyAClient.GetCompanyData(JurisdictionStrategyOrder.Uk.ToString().ToLower(), companyId, cancellationToken);

        return _mapper.Map(response);
    }
}